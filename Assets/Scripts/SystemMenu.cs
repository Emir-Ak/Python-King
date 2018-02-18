using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

//SystemConfigurationMenu manager, helds transitions and all settings
public class SystemMenu : MonoBehaviour
{
    //Declaration of class-wide variables
    #region VARIABLES
    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    GameObject mainMenuPrefab;
    [SerializeField]
    Toggle fullScreenToggle;

    [SerializeField]
    Button[] Buttons;

    Resolution[] resolutions;

    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    Slider SFXSlider;
    [SerializeField]
    Slider musicSlider;

    [SerializeField]
    List<MonoBehaviour> objectsInSystem = new List<MonoBehaviour>();
    [SerializeField]
    List<MonoBehaviour> objectsInAccount = new List<MonoBehaviour>();

    [SerializeField]
    GameObject systemSettings;
    [SerializeField]
    GameObject accountSettings;

    private float masterValue;
    private float SFXValue;
    private float musicValue;
    #endregion

    //Public methods for OnValueChanged events (Audio settings)
    #region AUDIO
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }
    #endregion

    //Public methods for OnValueChanged events (Video settings)
    #region VIDEO
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex]; //Assign the chosen resolution to the variable
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); //Apply the resolution
    }

    #endregion

    //Public methods for OnCick events
    #region BUTTONS
    public void ReturnButton()
    {
        //Transition to main menu    
        Instantiate(mainMenuPrefab); 
        SaveInput();
        Destroy(gameObject);
    }

    public void SystemButton()
    {
        MakeSectionTransition(true);
    }

    public void AccountButton()
    {
        MakeSectionTransition(false);
    }
    #endregion

    //Additional methods required
    #region ADDITIONAL
    void Start()
    {
        LoadInput();
        MakeSectionTransition(true);
    }

    //Manages transition between sections of SystemMenu 
    void MakeSectionTransition(bool toSettings)
    {
        systemSettings.SetActive(toSettings ? true : false);
        accountSettings.SetActive(toSettings ? false : true);
        ManageButtons(false);
        StartCoroutine(AnimateObjects());
    }

    //Save users system configuration change 
    void SaveInput()
    {
        masterValue = masterSlider.value;
        SFXValue = SFXSlider.value;
        musicValue = musicSlider.value;

        PlayerPrefs.SetFloat("MasterValue", masterValue);
        PlayerPrefs.SetFloat("SoundValue", SFXValue);
        PlayerPrefs.SetFloat("MusicValue", musicValue);
    }

    //Load the change
    void LoadInput()
    {
        masterValue = PlayerPrefs.GetFloat("MasterValue", masterValue);
        SFXValue = PlayerPrefs.GetFloat("SoundValue", SFXValue);
        musicValue = PlayerPrefs.GetFloat("MusicValue", musicValue);

        fullScreenToggle.isOn = Screen.fullScreen;

        SetClarifiedResolution();

        masterSlider.value = masterValue;
        SFXSlider.value = SFXValue;
        musicSlider.value = musicValue;
    }

    void SetClarifiedResolution()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        //For each resolution
        for (int i = 0; i < resolutions.Length; i++)
        {

            //Add available options to the list
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }

        }

        resolutionDropdown.AddOptions(options);             //Add options to the dropdown menu
        resolutionDropdown.value = currentResolutionIndex;  //Set the value to the current one you have
        resolutionDropdown.RefreshShownValue();             //Refresh the displayed value

        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// ALLERT: DON'T FORGET TO USE StartCoroutine()! ;-)
    /// </summary>
    IEnumerator AnimateObjects()
    {
        List<Text> texts = new List<Text>();
        List<GameObject> objects = new List<GameObject>();

        //Calculates which objects should be Text and which GameObject
        foreach (MonoBehaviour obj in (accountSettings.activeSelf == false ? objectsInSystem : objectsInAccount))
        {
            //Assigns to the relative lists
            if (obj.GetComponent<Text>() != null)
            {
                texts.Add(obj.GetComponent<Text>());
            }
            else
            {
                objects.Add(obj.gameObject);
            }
        }

        //Start animation
        StartCoroutine(AZAnim.AnimateMenu(this, texts, objects, 2f));
        yield return new WaitUntil(() => AZAnim.MenuAnimatingIsFinished);

        //Enable buttons
        ManageButtons(true);
    }

    //Disables or enables the buttons
    void ManageButtons(bool shouldBeInteractable)
    {
        foreach (Button button in Buttons)
        {
            if (shouldBeInteractable) button.interactable = true;
            else button.interactable = false;
        }
    }
    #endregion
}


