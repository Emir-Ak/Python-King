using UnityEngine.UI;
using UnityEngine;

public class PVEModeSettingsManager : MonoBehaviour
{
    [SerializeField]
    Scrollbar numberOfQuestionsScrollbar;
    [SerializeField]
    Scrollbar AILevelScrollbar;

    private Text numberOfQuestionsNumber;
    private Text AILevelNumber;
    private void Awake()
    {
        numberOfQuestionsNumber = numberOfQuestionsScrollbar.GetComponentInChildren<Text>();
        AILevelNumber = AILevelScrollbar.GetComponentInChildren<Text>();
    }
    private void Start()
    {
        numberOfQuestionsScrollbar.value = 7 / 10 - 1;
       
    }

    private void Update()
    {
        SetScrollbar(numberOfQuestionsScrollbar, numberOfQuestionsNumber, "NumberOfQuestions");
        SetScrollbar(AILevelScrollbar, AILevelNumber, "AILevel");
    }

    void SetScrollbar(Scrollbar sb, Text number, string playerPrefs)
    {

        float value = sb.value;

        if (sb.value != value)
        {
            numberOfQuestionsScrollbar.value = value;
            int finalCalculation = Mathf.FloorToInt(numberOfQuestionsScrollbar.value * 10);
            if (finalCalculation != 10)
            {
                finalCalculation++;
            }

            number.text = finalCalculation.ToString();
            PlayerPrefs.SetInt(playerPrefs, finalCalculation);
        }
    }

    void GetScrollbar(Scrollbar sb, Text number, string playerPrefs)
    {

        float value = PlayerPrefs.GetInt(playerPrefs);

        if (sb.value != value)
        {
            numberOfQuestionsScrollbar.value = value;
            int finalCalculation = Mathf.FloorToInt(numberOfQuestionsScrollbar.value * 10);
            if (finalCalculation != 10)
            {
                finalCalculation++;
            }

            number.text = finalCalculation.ToString();
            PlayerPrefs.SetInt(playerPrefs, finalCalculation);
        }
    }
}
