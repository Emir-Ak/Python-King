using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public GameObject[] mainMenuButtons;


    private void Awake()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.SetActive(false);
        }
    }

    private void Start()
    {
        Invoke("MakeButtonsVisible",6f);
    }

    void MakeButtonsVisible()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.SetActive(true);
        }
    }

    public void SetFullScreen(bool fullScreenMode)
    {
        Screen.fullScreen = fullScreenMode;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
