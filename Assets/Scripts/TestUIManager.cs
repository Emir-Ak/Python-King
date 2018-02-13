using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Manages the examination of the user
public class TestUIManager : MonoBehaviour
{
    //Declaration of class-wide variables
    #region VARIABLES
    public List<QAContainer> containers = new List<QAContainer>();

    [SerializeField]
    GameObject mainMenuPrefab;

    //Text spacefield for questions
    [SerializeField]
    Text currentQuestionText;

    //Everything related to answering the questions
    [SerializeField]
    InputField answerField;        //Where user enters the answer
    [SerializeField]
    Text correctAnswersText;       //Display for showing number of correct answers
    int currentQuestionIndex = 0;  //Index of a question, on which the user stopped (It will allows us to rerun the coroutine)
    int correctAnswers = 0;        //Number of correct answers

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations
    float timeAvailable;  //Time available to examiner for each question

    private IEnumerator startExaminationCoroutine; //Holds the coroutine

    bool checkingAnswer = false;

    [SerializeField]
    Text helperText; //"ENTER..." text
    [SerializeField]
    Animator helperTextAnimator;
    #endregion

    //Methods related to start function
    #region START
    private void Start()
    {
        answerField.ActivateInputField();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Coroutine is stored in a variable, with all attributes assigned
        startExaminationCoroutine = StartExamination();
        StartCoroutine(startExaminationCoroutine); //Starts the coroutine
        //Turns text into a form of Correct Answers/Numer of questions
        correctAnswersText.text = correctAnswers.ToString() + "/" + containers.Count.ToString();

        timeBar.gameObject.SetActive(false); //To refer to some features of UI element, you need too refer to it's "gameObject" property

    }
    #endregion

    //Methods related to update function
    #region UPDATE
    private void Update()
    {
        //Calculation of slider value change
        if (timeBarValue > 0f && timeBar.gameObject != null)
        {
            //Value decreased relative to time and changed into "0 to 1" form
            timeBarValue -= Time.deltaTime;
            timeBar.value = timeBarValue / timeAvailable;
        }

        if (answerField.isFocused == false)
        {
            answerField.ActivateInputField();
        }

        if (checkingAnswer == false)
        {
            answerField.text = string.Empty;
        }

        if (answerField.text != string.Empty)
        {
            helperTextAnimator.Play("HelperText");
            helperText.text = "ENTER...";
        }

        else
        {
            helperText.text = string.Empty;
        }
    }
    #endregion

    //Here is everything for managing the examination/testing logic
    #region EXAMINATION

    #region Main_Coroutine
    IEnumerator StartExamination()
    {
        //If any questions present...
        if (containers != null && containers.Count > 0)
        {

            //For every question in the list
            foreach (QAContainer container in containers)
            {
                //Skip all the questions that were seen already
                if (containers.IndexOf(container) != currentQuestionIndex)
                {
                    continue;
                }

                if (container.question == string.Empty || container.solution == string.Empty || container.time <= 0f)
                {
                    Debug.LogError("Either question or solution is empty or time is less then or equal to 0 (TestUI)");
                    Debug.Break();
                }


                checkingAnswer = false;

                StartCoroutine(AZAnim.TypeWrite(currentQuestionText, container.question, 2f)); //Type in the question

                //Internal variable, time for a question
                bool _Finished = false;

                yield return new WaitWhile(() => !AZAnim.TypeWritingIsFinished);   //Wait till the type-in animation is finished

                timeAvailable = container.time;              //Personal time for each question
                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                currentQuestionIndex++; //The question is seen, index increased

                //Limit the answer according to the length of the solution
                answerField.characterLimit = container.solution.Length;

                checkingAnswer = true;

                //Starts coroutine which checks user input answer
                StartCoroutine(CheckAnswer(container.solution, _Finished));

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
            Debug.LogError("No containers in the list");
            Debug.Break();
        }
    }
    #endregion

    #region Sub_Coroutine
    IEnumerator CheckAnswer(string solution, bool timeIsFinished)
    {
        if (solution == string.Empty)
        {
            Debug.LogError("No solution for this question assigned");
            Debug.Break();
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

                //Will make the loop act as Update();
                yield return null;
            }
        }
    }
    #endregion

    #region Other_Methods
    void ResetExaminationSettings()
    {
        currentQuestionText.text = string.Empty;
        correctAnswers = 0;
        currentQuestionIndex = 0;

        if (mainMenuPrefab == null)
            Debug.LogError("StartMenu is not assigned!");

        //Transition to main menu
        GameObject _instance;
        _instance = Instantiate(mainMenuPrefab);
        _instance.name = mainMenuPrefab.name;
        _instance = null;
        Destroy(gameObject);
    }
    #endregion

    #endregion
}
