using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

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

    Resolution[] resolutions;
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
        MakeTransition(mainMenuPrefab); //Transition to MainMenu
    }
    #endregion

    //Additional methods required
    #region ADDITIONAL
    void Start()
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

            //Set appropriate resolution for the screen
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);             //Add options to the dropdown menu
        resolutionDropdown.value = currentResolutionIndex;  //Set the value to the current one you have
        resolutionDropdown.RefreshShownValue();             //Refresh the displayed value

        fullScreenToggle.isOn = Screen.fullScreen;
    }

    void MakeTransition(GameObject prefab)
    {
        //This is used to get rid of the "(Clone)" part in the name, to keep hierarchy tidy
        GameObject _instance;            //Create internal variable of type GameObject
        _instance = Instantiate(prefab); //Instantiate prefab, assign it to the variable
        _instance.name = prefab.name;    //Rename it to original name
        _instance = null;                //Free the variable

        Destroy(gameObject);
    }
    #endregion
}



