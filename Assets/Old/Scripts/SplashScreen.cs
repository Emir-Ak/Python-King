using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
public class SplashScreen : MonoBehaviour {

    public float timeBeforeLoad;

    private void Awake()
    {
        StartCoroutine(SetFullScreen());
    }

    void Start () {
        Invoke("LoadScene", timeBeforeLoad);
	}


	void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator SetFullScreen()
    {
        yield return new WaitForSeconds(timeBeforeLoad - 0.2f);
        if(Screen.fullScreen == false)
        {
            Screen.fullScreen = true;
        }

        yield return new WaitForSeconds(0.1f);
        Screen.fullScreen = false;
    }
    

}
