using UnityEngine;            
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestManager : MonoBehaviour
{
    #region Variables
    public List<QAContainer> containers = new List<QAContainer>();

    //Text spacefield for questions
    [SerializeField]
    Text currentQuestionText;

    [SerializeField]
    InputField answerField;        //Where user enters the answer
    [SerializeField]
    Text correctAnswersText;       //Display for showing number of correct answers
    int correctAnswers = 0;        //Number of correct answers
    int currentQuestionIndex = 0;  /*Index of a question, on which the user stopped 
                                      (It will allows us to rerun the coroutine) */

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations
    float timeAvailable;  //Time available to examiner for each question

    private IEnumerator startExaminationCoroutine; //Holds the coroutine

    [SerializeField]
    GameObject startMenuPrefab;
    private GameObject startMenuInstance;
    #endregion

    #region Start
    private void Start()
    {
        //Coroutine is stored in a variable, with all attributes assigned
        startExaminationCoroutine = StartExamination();
        StartCoroutine(startExaminationCoroutine); //Starts the coroutine
        //Turns text into a form of Correct Answers/Numer of questions
        correctAnswersText.text = correctAnswers.ToString() + "/" + containers.Count.ToString();

        timeBar.gameObject.SetActive(false); //To refer to some features of UI element, you need too refer to it's "gameObject" property

    }

    #endregion
    #region Update
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

    #endregion
    #region Examination
    IEnumerator StartExamination()
    {
        //If any questions present...
        if (containers != null)
        {

            //For every question in the list
            foreach (QAContainer container in containers)
            {
                //Skip all the questions that were seen already
                if (containers.IndexOf(container) != currentQuestionIndex)
                {
                    continue;
                }

                //Limit the answer according to the length of the solution
                answerField.characterLimit = container.solution.Length;

                StartCoroutine(TypeWriter.TypeWrite(currentQuestionText, container.question)); //Type in the question

                //Internal variable, time for a question
                bool _Finished = false;

                yield return new WaitWhile(() => !TypeWriter.TypingIsFinished);   //Wait till the type-in animation is finished

                timeAvailable = container.time;              //Personal time for each question
                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                //Starts coroutine which checks user input answer
                StartCoroutine(CheckAnswer(container.solution, _Finished));

                currentQuestionIndex++; //The question is seen, index increased

                yield return new WaitForSeconds(timeAvailable); //Wait before next question

                answerField.text = string.Empty; //clear the previous answer

                _Finished = true; //Time for question is finished
                                       //(Passed to "CheckAnswer" coroutine)

                timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0
            }

            ResetExaminationSettings(); //Will reset all settings to be able to start test again
        }

        else
        {
            Debug.LogError("No containers in a list");
        }
    }

    IEnumerator CheckAnswer(string solution, bool timeIsFinished)
    {
        if (solution == string.Empty)
        {
            Debug.LogError("No solution for this question assigned");
        }

        else
        {
            //Executes till the time for a question is finished
            while (timeIsFinished == false)
            {

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //if answer is correct
                    if (answerField.text == solution)
                    {
                        correctAnswers++;
                    }

                    StopCoroutine(startExaminationCoroutine);  //Stop coroutine on the question it is on
                    StartCoroutine(startExaminationCoroutine); //Start it again, but currentQuestionIndex fixes it

                    //Turns text into a form of Correct Answers/Numer of questions
                    correctAnswersText.text = correctAnswers.ToString() + "/" + containers.Count.ToString();

                    break;
                }

                //Will make the loop act as FixedUpdate()
                //Better timing will be achieved in future to increase performance
                yield return new WaitForFixedUpdate();
            }
        }
    }

    void ResetExaminationSettings()
    {
        currentQuestionText.text = string.Empty;
        correctAnswers = 0;
        currentQuestionIndex = 0;

        if (startMenuPrefab == null)
            Debug.LogError("StartMenu is not assigned!");

        startMenuInstance = Instantiate(startMenuPrefab);
        startMenuInstance.name = startMenuPrefab.name;

        Destroy(gameObject);
    }

    #endregion  
}
