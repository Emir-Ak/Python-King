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
    public List<int> rndInts = new List<int>();

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
    SecureInt correctAnswers;        //Number of correct answers
    
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
        SetRndIntsList();

        correctAnswers = new SecureInt(0);

        helperTextAnimator.speed = 0f;
        answerField.ActivateInputField();         //Focus on input field
        Cursor.visible = false;                   //Cursor is not visible
        Cursor.lockState = CursorLockMode.Locked; //Lock the cursor in the middle

        //Coroutine is stored in a variable, with all attributes assigned
        startExaminationCoroutine = StartExamination();
        StartCoroutine(startExaminationCoroutine); //Starts the coroutine
        //Turns text into a form of Correct Answers/Numer of questions
        correctAnswersText.text = correctAnswers.ToString() + "/" + containers.Count.ToString();

        timeBar.gameObject.SetActive(false); //To refer to some features of UI element, you need too refer to it's "gameObject" property

        //Clears the helper text
        helperText.text = string.Empty;

    }

    void SetRndIntsList()
    {
        int _subNumber = Random.Range(0, containers.Count);

        //Set a List of random numbers, with Lenght equal to number of quesions
        for (int i = 0; i < containers.Count; i++)
        {
            while (rndInts.Contains(_subNumber))
            {
                _subNumber = Random.Range(0, containers.Count);
            }
            rndInts.Add(_subNumber);
        }
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

        //If the user is not focused on input field
        if (answerField.isFocused == false)
        {
            //Activate it and force him to focus onto it
            answerField.ActivateInputField();
        }

        //If answer is not being checked
        if (checkingAnswer == false)
        {
            //Delete everything user types in
            answerField.text = string.Empty;
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
            foreach (int i in rndInts)
            {

                //Skip all the questions that were seen already
                if (rndInts.IndexOf(i) != currentQuestionIndex)
                {
                    continue;
                }

                if (containers[i].question == string.Empty || containers[i].solution == string.Empty || containers[i].time <= 0f)
                {
                    Debug.LogError("Either question or solution is empty or time is less then or equal to 0 (TestUI)");
                }

                helperText.text = string.Empty;

                checkingAnswer = false;

                StartCoroutine(AZAnim.TypeWrite(currentQuestionText, containers[i].question, 2f)); //Type in the question

                //Internal variable, time for a question
                bool _Finished = false;

                yield return new WaitWhile(() => !AZAnim.TypeWritingIsFinished);   //Wait till the type-in animation is finished

                timeAvailable = containers[i].time;   //Personal time for each question
                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                currentQuestionIndex++; //The question is seen, index increased

                //Limit the answer according to the length of the solution
                answerField.characterLimit = containers[i].solution.Length;

                checkingAnswer = true;

                yield return new WaitForSeconds(0.1f);

                //Starts coroutine which checks user input answer
                StartCoroutine(CheckAnswer(containers[i].solution, _Finished));

                yield return new WaitForSeconds(timeAvailable); //Wait before next question

                answerField.text = string.Empty; //clear the previous answer

                //Time for question is finished (Passed to "CheckAnswer" coroutine)
                _Finished = true; 

                timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0
            }

            ResetAndExit(); //Will reset all settings to be able to start test again
        }

        else
        {
            Debug.LogError("No containers in the list");
        }
    }
    #endregion

    #region Sub_Coroutines
    IEnumerator CheckAnswer(string solution, bool timeIsFinished)
    {
        if (solution == string.Empty)
        {
            Debug.LogError("No solution for this question assigned");
        }

        else
        {
            bool _isAnswerFull = false;
            //Executes till the time for a question is finished
            while (timeIsFinished == false)
            {
                //If answer is as long as solution is
                if (answerField.text.Length == solution.Length)
                {
                    helperTextAnimator.speed = 1f;
                    if (_isAnswerFull == false)
                    {
                        _isAnswerFull = true;
                        //Start animation, make the helper text visible
                        StartCoroutine(HelperTextAnimation());
                    }

                    if (helperText.text != "ENTER...")
                    {
                        helperText.text = "ENTER...";
                    }
                }
                else
                {
                    //Delete the helper text
                    helperText.text = string.Empty;
                    _isAnswerFull = false;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    //if answer is correct
                    if (answerField.text == solution)
                    {
                        correctAnswers += new SecureInt(1);
                    }

                    StopCoroutine(startExaminationCoroutine);  //Stop coroutine on the question it is on
                    yield return new WaitForSeconds(0.05f);
                    StartCoroutine(startExaminationCoroutine); //Start it again, and currentQuestionIndex fixes it

                    //Turns text into a form of Correct Answers/Numer of questions
                    correctAnswersText.text = correctAnswers.ToString() + "/" + containers.Count.ToString();

                    break;
                }

                //Will make the loop act as FixedUpdate();
                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator HelperTextAnimation()
    {
        //Restart the anuimation
        helperTextAnimator.Play("HelperTextFadeIn");
        yield return null;
        helperTextAnimator.Play("HelperTextIdle");
    }
    #endregion

    #region Other_Methods

    void ResetAndExit()
    {
        currentQuestionText.text = string.Empty;
        correctAnswers = new SecureInt(0);
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

    //Public methods for events
    #region EVENTS
    public void LimitAnswerInput()
    {
        string _limitedInput = AZAnim.LimitString(answerField.text, 0, -1, true);
        answerField.text = _limitedInput;
    }
    #endregion


}
