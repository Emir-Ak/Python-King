using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SpriteChange : MonoBehaviour
{
    float timeBeforeChange;

    public float timeForAnimation = 2;
    public float timeBeforeNextSprite = 0;
    public float delay = 0;

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

            yield return new WaitForSeconds(timeForAnimation);
            yield return new WaitForSeconds(timeBeforeChange);

            foreach (Sprite i in spriteList)
            {
                image.sprite = i;
                image.color = new Color(255,255,255,0);
                image.DOFade(1, timeForAnimation);
                yield return new WaitForSeconds(timeForAnimation);
                if (i != spriteList[spriteList.Count - 1])
                {
                    yield return new WaitForSeconds(timeBeforeChange);
                    image.DOFade(0, timeForAnimation);
                    yield return new WaitForSeconds(timeForAnimation);
                }

            }


    }
}
