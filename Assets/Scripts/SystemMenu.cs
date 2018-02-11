using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

//SystemConfigurationMenu manager, helds transitions and all settings
public class SystemMenu : MonoBehaviour
{
    //No comments (if the name is "VARIABLES", then the comment is "No comments" :D)
    #region NAME_ME_PLEASE
    //AND COMMENT ALL OF US WHEN YOU WILL FINISH WITH US!!!
    //Stephan, when regioning keep the capitalisation, and put small description after each main region :"//All methods to do with resolution", and also add subregions (different way of writing!!!) if needed
    //So please open all regions in TestUIManager script and look how it is done there, main region - 1 word only, capital letters :"EXAMINATION"   ;   subregions - name-like capitalisation, "_" as a space :"Main_Coroutine".
    //P.S. - :"text" is an example.

    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    GameObject mainMenuPrefab;

    Resolution[] resolutions;
    #endregion

    //Sub-helping methods I guess... 
    #region #ИМЯВСТУДИЮ
    //Or wtv u call it, but make it obvious for new members to understand it as it will help in our "real" project ;)

    //Always used for transition buttons (When animated UI, animation will start again)
    void MakeTransition(GameObject prefab)
    {
        //This is used to get rid of the "(Clone)" part in the name, to keep hierarchy tidy
        GameObject _instance;
        _instance = Instantiate(prefab);
        _instance.name = prefab.name;
        _instance = null;

        Destroy(gameObject);
    }
    #endregion

    //Something not to look stupid (uncreative)
    #region REGION_ME_AS_WELL

    //No comments, they are only in main regions and 
    //might be inside the subregion (under it)
    //sometimes above the methods also if
    //the name is not too obvious (like for example "MoveTextToRightOrLeftRelativeToHisInput()")

    //Public methods needed for click events (Only one for now)
    #region Buttons
    public void ReturnButton()
    {
        MakeTransition(mainMenuPrefab);
    }
    #endregion

    #region Sub_Region

    void Start()
    //"Methods related to start function" (RENAME ME!!! I dont like when other programmists interfer with naming what YOU CREATED (Emir - Aha, just copied from Brackeys xDXDxDXD lol)!!)
    //Emir - but really u can keep it if u want, cuz I wrote the same, and its kinda what it should be like, straight to the point I mean

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
    #endregion

    #region Audio_Settings
    //All methods related to the audi... WHATS THE FUCK EMIR?!??! HOW DID YOU DARE TO PUT YOUR OWN NAME TO MY SUBREGION AND ITS DESCRIPTION!!!??!??!? 
    //*Looks at the name of the region*..
    //...
    //....
    //...
    //......
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //....
    //....
    //....
    //*Looks at the name again...*
    //..
    //.....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //..
    //....
    //....
    //*Thinks something*
    //...
    //...
    //...
    //...
    //..
    //...
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //...
    //...
    //...
    //...
    //...
    // *RECONVINCES HIM SELF MULTIPLE TIMES*
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //...
    //..
    //.
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    // Блять откуда так много точе... Khm khm.. 
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    // - I think you skiped a letter up there...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //....
    //....
    //....
    //..
    //.
    //....    //.. (Hmmm??..)
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....    //.. (Wtf)
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    // LOL ТЕБЯ НАЕ***И, ладно вернемся к основному сюжету
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //..
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //.
    //....
    //..
    //..
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //..
    //..
    //..
    //...
    //..
    //....
    // *Several hours passed*
    //..
    //..
    //..
    //...
    //..
    //....
    //....
    //....
    //....
    //....
    //....
    //..
    //...
    //..
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //....
    //..
    //...
    //..
    //....
    //..
    //...
    //..
    //..
    //...
    //..
    //..
    //...
    //..
    //..
    //...
    //..
    //..
    //...
    //..
    //..
    //...
    //..
    //....
    //....
    // (Stephan thinks) - "You know what, the name is not that bad actually, maybe I will keep it..." xD

       
  


























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

    #region Master_Volume
    //Blyat, hi pyt ze rong name, suka. I vill chandzh ит to muч bitter 1
    //WON'T I?

    //me - Dought that you even realised it xD












    //*Thinks* - realised what?...

    //...
    //...
    //...
    //*Few minutes of awkward silence passed*
    //..


    //Emir (Starts dancing like in PPOP)- I have a face, I have a palm... 


    //(Puts hands together, one into another) - ahm




    /*That was the last comment from me, I promise! :P*/
 
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
















































    //No it wasn't! lol ur face looks so funny when you realise that someone was lying to you :p
    #endregion
    #endregion

    //Whatever description
    #region WHATEVER_NAME


    //Teacher - 33 + 33?


    //Stephan (writes on the board) - 69

























































    //T - GET OUT OF HERE YOU HENTAII!!!







































    //S - Omae wa mou shindeiru!!!
















    //T - You spelled it wrong


    //S - Miss, you had to go like "NAAAANIII" *Spits all over the teacher*


    //...



    //T- Deep inside himself, like really really deep, somewhere very very down there inside his soul... 
    //...he is a clever person...



    //:P
    #endregion
}



