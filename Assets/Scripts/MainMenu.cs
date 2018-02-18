using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
//MainMenu manager, helds all button transitions
public class MainMenu : MonoBehaviour
{
    //Declaration of class-wide variables
    #region VARIABLES
    [SerializeField]
    GameObject testUIPrefab;
    [SerializeField]
    GameObject systemMenuPrefab;
    [SerializeField]
    GameObject logMenuPrefab;

    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Button[] buttons;
    List<Text> buttonTexts = new List<Text>();

    #endregion

    //Public methods needed for OnClick events
    #region BUTTONS
    public void CompileButton()
    {
        MakeTransition(testUIPrefab);
    }
    public void SystemButton()
    {
        MakeTransition(systemMenuPrefab);
    }
    public void LogButton()
    {
        MakeTransition(logMenuPrefab); 
    }
    public void BreakButton()
    {
        Application.Quit();
    }
    #endregion

    //Additional methods required
    #region ADDITIONAL
    private void Update()
    {
        if (AZAnim.MenuAnimatingIsFinished == true && buttons[0].interactable == false)
        {
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        LoadInput();

        foreach (Button button in buttons)
        {
            buttonTexts.Add(button.GetComponentInChildren<Text>());
            button.interactable = false;
        }

        StartCoroutine(AZAnim.AnimateMenu(this, buttonTexts, null, 0.5f));
    }

    void LoadInput()
    {
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterValue"));
        audioMixer.SetFloat("SoundVolume", PlayerPrefs.GetFloat("SoundValue"));
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicValue"));
    }

    //Used for transition buttons
    void MakeTransition(GameObject prefab)
    {
        Instantiate(prefab);
        Destroy(gameObject);
    }
    #endregion
}
