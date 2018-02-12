using UnityEngine.UI;
using UnityEngine;

public class SectionButtonCustomization : MonoBehaviour
{

    Image image;

    int sectionIndex;

    private void Start()
    {
        image = GetComponent<Image>();
        sectionIndex = 0;
        PlayerPrefs.SetInt("sectionIndex", sectionIndex);
        SetColor();
    }

    private void Update()
    {
        SetColor();
    }

    void SetColor()
    {
        sectionIndex = PlayerPrefs.GetInt("sectionIndex");
        if (sectionIndex == 0)
        {
            if (gameObject.name == "SystemButton")
                image.color = new Color(0, 0, 0, 0);

            else
                image.color = new Color(0, 0, 0, 25);
        }
        else
        {
            if (gameObject.name == "AccountButton")
                image.color = new Color(0, 0, 0, 0);

            else
                image.color = new Color(0, 0, 0, 25);
        }
    }

    public void ChooseSection()
    {
        if (gameObject.name == "SystemButton")
        {
            sectionIndex = 0;
        }
        else
        {
            sectionIndex = 1;
        }
        PlayerPrefs.SetInt("sectionIndex", sectionIndex);
    }
}
