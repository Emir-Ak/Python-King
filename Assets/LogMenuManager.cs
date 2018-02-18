using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMenuManager : MonoBehaviour {

    [SerializeField]
    GameObject mainMenuPrefab;
    [SerializeField]
    List<Text> texts = new List<Text>();
    [SerializeField]
    List<GameObject> gameObjects = new List<GameObject>(); 
    public void ReturnToMenu()
    {
        Instantiate(mainMenuPrefab);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(AZAnim.AnimateMenu(this, texts, gameObjects, 3f));
    }

}
