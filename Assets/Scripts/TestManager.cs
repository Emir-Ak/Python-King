using UnityEngine;            
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TestManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    List<Question> questions = new List<Question>();

    //Text spacefield for questions
    [SerializeField]
    Text currentQuestionText;

    [SerializeField]
    InputField answerField;        //Where user enters the answer
    [SerializeField]
    Text correctAnswersText;       //Display for showing number of correct answers
    int correctAnswers = 0;        //Number of correct answers
    int currentQuestionIndex = 0;  //Index of a question, on which the user stopped 
                                   //(It will allows us to rerun the coroutine)

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations
    float timeAvailable;  //Time available to examiner for each question

    int solutionLength;   //Length of the solution

    private IEnumerator startExaminationcoroutine;

    [SerializeField]
    GameObject startMenuPrefab;
    private GameObject startMenuInstance;

    #endregion
    #region Start
    private void Start()
    {
        //Coroutine is stored in a variable, with all attributes assigned
        startExaminationcoroutine = StartExamination(questions, currentQuestionText, answerField);
        StartCoroutine(startExaminationcoroutine); //Starts the coroutine
        //Turns text into a form of Correct Answers/Numer of questions
        correctAnswersText.text = correctAnswers.ToString() + "/" + questions.Count.ToString();

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
    IEnumerator StartExamination(List<Question> listOfQuestions, Text spaceForQuestions, InputField answer)
    {
        //If any questions present...
        if (listOfQuestions != null)
        {
            //For every question in the list
            foreach (Question question in listOfQuestions)
            {
                //Skip all the questions that were seen already
                if (listOfQuestions.IndexOf(question) != currentQuestionIndex)
                {
                    continue;
                }
                //Limit the answer according to the length of the solution
                answer.characterLimit = question.solution.Length;

                StartCoroutine(TypeWriter.TypeWrite(spaceForQuestions, question.pythonQuestion)); //Type in the question

                //Internal variable, time for a question
                bool _didTimeFinish = false;

                yield return new WaitWhile(() => !TypeWriter.TypingIsFinished);   //Wait till the type-in animation is finished

                timeAvailable = question.time;        //Personal time for each question
                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                //Starts coroutine which checks user input answer
                StartCoroutine(CheckAnswer(answer, question.solution, _didTimeFinish));

                currentQuestionIndex++; //The question is seen, index increased

                yield return new WaitForSeconds(timeAvailable); //Wait before next question

                answer.text = string.Empty; //clear the previous answer

                _didTimeFinish = true; //Time for question is finished
                                       //(Passed to "CheckAnswer" coroutine;

                timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0

                Debug.Log("Timeout");
            }

            Debug.Log("Resseting");
            ResetEverything(spaceForQuestions); //Will reset all settings to be able to start test again
        }

        else
        {
            Debug.LogError("No questions in a list (StartExamination)");
        }
    }

    IEnumerator CheckAnswer(InputField inputField, string solution, bool didTimeFinish)
    {
        if (solution == string.Empty)
        {
            Debug.LogError("No solution for this question assigned");
        }

        else
        {
            //Executes till the time for question is finished
            while (didTimeFinish == false)
            {

                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    //if answer is correct...
                    if (inputField.text == solution)
                    {
                        correctAnswers++;
                    }

                    StopCoroutine(startExaminationcoroutine);  //Stop coroutine on the question it is on
                    StartCoroutine(startExaminationcoroutine); //Start it again, but currentQuestionIndex fixes it
                    //Turns text into a form of Correct Answers/Numer of questions
                    correctAnswersText.text = correctAnswers.ToString() + "/" + questions.Count.ToString();

                    break;
                }

                //Will make the loop act as FixedUpdate()
                //Better timing will be achieved in future to increase performance
                yield return new WaitForFixedUpdate();
            }
        }
    }

    void ResetEverything(Text text)
    {
        text.text = string.Empty;
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
