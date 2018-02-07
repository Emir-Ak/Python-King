using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteChange : MonoBehaviour
{
    private float timeBeforeNextSprite = 0f;
    private float delay = 0f;

    Image image;

    [Header("Sprites that will change through animation")]

    public List<Sprite> spriteList;

    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(SpriteChangeCoroutine());
    }
    IEnumerator SpriteChangeCoroutine()
    {
            yield return new WaitForSeconds(delay);

            yield return new WaitForSeconds(timeBeforeNextSprite);

            foreach (Sprite i in spriteList)
            {
                image.sprite = i;
                image.color = new Color(255,255,255,0);
                if (i != spriteList[spriteList.Count - 1])
                {
                    yield return new WaitForSeconds(timeBeforeNextSprite);
                }

            }


    }
}
