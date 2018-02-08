using UnityEngine;            
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestManager : MonoBehaviour {

    [SerializeField]
    List<string> questions = new List<string>();

    //Text spacefield for questions
    [SerializeField]
    Text currentQuestionText;

    //Time available to examiner for 1 question
    [Tooltip("TIme available for one question")]
    public float timeAvailable;

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations

    private void Start()
    {
        //Starts the coroutine at start (later will be activated by a button)
        StartCoroutine(StartExamination(questions, currentQuestionText, timeAvailable));

        timeBar.gameObject.SetActive(false); //To refer to some features of UI element, you need too refer to it's "gameObject" property
    }

    private void Update()
    {
        //Calculation of slider value change
        if (timeBarValue > 0f && timeBar.gameObject != null)
        {
            //Value decreased relative to time and changed into "0 to 1" form
            timeBarValue -= Time.deltaTime;
            timeBar.value = timeBarValue / timeAvailable;
        }
    }

    IEnumerator StartExamination(List<string> listOfQuestions, Text spaceForQuestions, float timeForQuestion)
    {

        //If any questions present...
        if (listOfQuestions != null) {

            //For every question in the list
            foreach (string question in listOfQuestions)
            { 
                StartCoroutine(TypeWriter.TypeWrite(spaceForQuestions, question)); //Type in the question
                yield return new WaitWhile(() => !TypeWriter.TypingIstFinished);   //Wait till the type-in animation is finished

                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                yield return new WaitForSeconds(timeForQuestion); //Wait before next question

                timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0
            }
        }

        else
        {
            Debug.LogError("No questions in a list (StartExamination)");
        }
    }
}
