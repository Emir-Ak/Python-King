using UnityEngine.UI;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    [SerializeField]
    GameObject testUIPrefab;
    private GameObject testUIInstance;

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

    public void Log()
    {
        //Sets a text displaying players info, and sets its self inactive (Any text for now), but also a logo next to the text and "RETURN" button. (PS, return has to be everywhere except TestUI...)
    }

    public void Break()
    {
        Application.Quit();
    }
}
