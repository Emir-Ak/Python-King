using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField]
    Image fillImg;     //Image to fill reltive to progress
    [SerializeField]
    Text progressText;  //Percentage progress text

    float i = 0;

    private void Start()
    {
        AZAnim.AnimateScale(fillImg.gameObject, 2f);
        StartCoroutine(AsyncSceneLoad());
    }

    IEnumerator AsyncSceneLoad()
    {

        while (fillImg.fillAmount != 1)
        {
            fillImg.fillAmount += 0.01f;
            if (int.Parse(progressText.text) < 100f)
                progressText.text = (Mathf.Round(i += 1f)).ToString();
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(AZAnim.AnimateScale(fillImg.gameObject, Time.fixedDeltaTime, 1, 0.35f));
        yield return new WaitForSeconds(1f);
        //Get next scene's ID
        int _sceneID = SceneManager.GetActiveScene().buildIndex + 1;
        //Asynchronised scene loading operation is assigned to a variable
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneID);
    }
}
