using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{

    [SerializeField]
    GameObject testUIPrefab;
    private GameObject testUIInstance;

    public AudioMixer audioMixer;
    [SerializeField]
    public Dropdown resolutionDropdown;

    


    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Awake() //Never put this to start
    {
        if (testUIPrefab == null)
            Debug.LogError("TestUI is not assigned!");
    }

    public void Compile()
    {
        testUIInstance = Instantiate(testUIPrefab);
        testUIInstance.name = testUIPrefab.name;
        Destroy(gameObject);
    }

    public void System()
    {
        //Inactive self, but before that call a panel which would have a volume mixer (use given audio from audio folder to test for now), reolution display, fullscreen mode and account settings button (only button for now..) - Use brackeys channel, pause menu video to make everything properly and neat
    }

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

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Log()
    {
        //Sets a text displaying players info, and sets its self inactive (Any text for now), but also a logo next to the text and "RETURN" button. (PS, return has to be everywhere except TestUI...)
    }

    public void Break()
    {
        Application.Quit();
    }
}
