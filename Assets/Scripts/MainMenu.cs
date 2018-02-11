using UnityEngine;

//MainMenu manager, helds all button transitions
public class MainMenu : MonoBehaviour
{
    //No comments
    #region VARIABLES
    [SerializeField]
    GameObject testUIPrefab;
    [SerializeField]
    GameObject systemMenuPrefab;
    //[SerializeField]  -Uncomment when log prefab is ready
    //logMenuPrefab     -Uncomment when log prefab is ready
    #endregion

    //Public methods needed for click events
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
        //MakeTransition(logMenuPrefab);  -Uncomment when log prefab is ready
    }

    public void BreakButton()
    {
        Application.Quit();
    }
    #endregion

    //This is the region for additional methods required
    #region ADDITIONAL
    //For now it is needed to keep the hierarchy and tidiness

    //Used for transition buttons
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
}
