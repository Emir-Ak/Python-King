using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SwitchGameObjects : MonoBehaviour {
    [SerializeField]
    GameObject objectToClose, objectToOpen;

    public Text[] textsToDisable;
    public Text[] textsToEnable;

    public float timeBeforeOpen = 0.25f, timeBeforeClose = 0.25f;

    public void SwitchObjects()
    {
        StartCoroutine(SwitchObjectsCoroutine());
    }

    IEnumerator SwitchObjectsCoroutine()
    {
        yield return new WaitForSeconds(timeBeforeOpen);

        objectToOpen.SetActive(true);

        if (textsToDisable != null) {
            foreach (Text i in textsToDisable)
            {
                i.enabled = false;
                i.text = "";
            }
        }

        if (textsToEnable != null) {
            foreach (Text i in textsToEnable)
            {
                i.enabled = true;
            }
        }

        yield return new WaitForSeconds(timeBeforeClose);

        objectToClose.SetActive(false);
    }
}
