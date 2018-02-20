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
    List<int> rndInts = new List<int>();

    [SerializeField]
    GameObject mainMenuPrefab;
    [SerializeField]
    GameObject modeChoice;
    [SerializeField]
    GameObject PVEModeSettings;
    //[SerializeField]
    //GameObject PVPModeSttings;
    [SerializeField]
    GameObject examination;

    //Text spacefield for questions
    [SerializeField]
    Text currentQuestionText;

    //Everything related to answering the questions
    [SerializeField]
    InputField answerField;        //Where user enters the answer
    [SerializeField]
    Text correctAnswersText;       //Display for showing number of correct answers
    int currentQuestionIndex = 0;  //Index of a question, on which the user stopped (It will allows us to rerun the coroutine)
    SecureInt correctAnswers;      //Number of correct answers

    [SerializeField]
    Text AICorrectAnswersText;     //Display for showing number of AI correct answers
    SecureInt AICorrectAnswers;    //Number of AI correct answers

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations
    float timeAvailable;  //Time available to examiner for each question

    private IEnumerator startExaminationCoroutine; //Holds the coroutine

    bool checkingAnswer = false;
    bool didTimeFinish;

    [SerializeField]
    Text helperText; //"ENTER..." text
    [SerializeField]
    Animator helperTextAnimator;

    //Names give themselfs out... 
    [SerializeField]
    Scrollbar numberOfQuestionsScrollbar;
    [SerializeField]
    Scrollbar AILevelScrollbar;
    Text numberOfQuestionsNumberText;
    Text AILevelNumberText;

    [SerializeField]
    Text resultText;

    private SecureFloat AILevel;
    private int numberOfQuestions;
    #endregion

    private void Start()
    {
        PVEModeSettings.SetActive(false);
        StartCoroutine(AZProtection.DetectCheatProcessesCoroutine(10f, true, 2f));
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
        if(didTimeFinish == true)
        {
            Debug.Log("TimeShouldFinish");
        }
    }

    //Here are all private the methods
    #region METHODS
    IEnumerator StartExamination()
    {


        //For every question in the list
        foreach (int i in rndInts)
        {
            checkingAnswer = false;

            //Skip all the questions that were seen already
            if (rndInts.IndexOf(i) != currentQuestionIndex)
            {
                continue;
            }

            if (rndInts.IndexOf(i) > numberOfQuestions - 1)
            {
                StartCoroutine(DisplayResult());
            }

            helperText.text = string.Empty;

            StartCoroutine(AZAnim.TypeWrite(currentQuestionText, containers[i].question, 2f)); //Type in the question

            yield return new WaitUntil(() => AZAnim.TypeWritingIsFinished);   //Wait till the type-in animation is finisheds
                                                                              //Internal variable, time for a question
            didTimeFinish = false;


            timeAvailable = containers[i].time;   //Personal time for each question
            timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
            timeBar.gameObject.SetActive(true);   //Slider is visible now

            currentQuestionIndex++; //The question is seen, index increased

            //Limit the answer according to the length of the solution
            answerField.characterLimit = containers[i].solution.Length;

            checkingAnswer = true;
            //Starts coroutine which checks user input answer
            StartCoroutine(CheckAnswer(containers[i].solution));

            yield return new WaitForSeconds(timeAvailable); //Wait before next question

            CoroutineControl();

            answerField.text = string.Empty; //clear the previous answer

            float chance = Random.value;

            timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0
           
            //Helds all AI percentage calculations and displays it
            if (chance > (1 - (AILevel.GetValue() / 10 - (AILevel.GetValue() == 10 ? 0.01f : 0) * AILevel.GetValue())))
            {
                AICorrectAnswers = AICorrectAnswers + new SecureInt(1);
                AICorrectAnswersText.text = "AI: " + AICorrectAnswers.ToString() + "/" + numberOfQuestions;
            }

        }

        StartCoroutine(DisplayResult());

    }

    
    void CoroutineControl()
    {
        //Time for question is finished (Passed to "CheckAnswer" coroutine)
        didTimeFinish = true;
        Debug.Log("Time Finished2");
    }
    IEnumerator CheckAnswer(string solution)
    {

        bool _isAnswerFull = false;

        //Executes till the time for a question is finished
        while (didTimeFinish == false)
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

                helperText.text = "ENTER...";
            }
            else
            {
                //Delete the helper text
                helperText.text = string.Empty;
                _isAnswerFull = false;
            }
            if (didTimeFinish == true)
            {
                Debug.Log(didTimeFinish);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {

                //if answer is correct
                if (answerField.text == solution)
                {
                    correctAnswers += new SecureInt(1);
                }
                StopCoroutine(startExaminationCoroutine);  //Stop coroutine on the question it is on
                StartCoroutine(startExaminationCoroutine); //Start it again, and currentQuestionIndex fixes it
                //Turns text into a form of CorrectAnswers/Numer of questions
                correctAnswersText.text = correctAnswers.ToString() + "/" + numberOfQuestions;
                break;
            }

            //Will make the loop act as FixedUpdate();
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("T*()#*(304324m329[0)");
    }

    IEnumerator HelperTextAnimation()
    {
        //Restart the anuimation
        helperTextAnimator.Play("HelperTextFadeIn");
        yield return null;
        helperTextAnimator.Play("HelperTextIdle");
    }

    IEnumerator DisplayResult()
    {
        examination.SetActive(false);
        if (correctAnswers > AICorrectAnswers)
        {
            resultText.color = new Color32(9, 255, 21,255);
            StartCoroutine(AZAnim.TypeWrite(resultText, "CONGRATULATIONS!;" + "YOU WON " + correctAnswers.ToString() + " AGAINST " + AICorrectAnswers.ToString() + " OUT OF " + numberOfQuestions.ToString() + (AILevel.GetValue() == 10 && numberOfQuestions >= 9 ? "!;(Didn't expect that from you :3...)" : "!"), 2));
        }
        else if (correctAnswers.GetValue() == AICorrectAnswers.GetValue())
        {
            resultText.color = new Color32(255, 248, 9,255);
            StartCoroutine(AZAnim.TypeWrite(resultText, "It's a " + correctAnswers.ToString() + " to " + AILevel.ToString() + " DRAW" + (AILevel.GetValue() == 10 && numberOfQuestions >= 9 ? "!;(You were pretty good at this though xD)" : "..."), 2));
        }
        else
        {
            resultText.color = new Color32(255, 11, 11, 255);
            if (AILevel.GetValue() > 1)
            {
                StartCoroutine(AZAnim.TypeWrite(resultText, "Hmm..." + ";You lost " + numberOfQuestions.ToString() + " against " + correctAnswers.ToString() + ",;with " + numberOfQuestions + " questions total" + (AILevel.GetValue() == 10 && numberOfQuestions >= 9 ? "...;(Dont worry, I know conditions;weren't fair on this one)" : "..."), 2));
            }
            else
            {
                StartCoroutine(AZAnim.TypeWrite(resultText, "Hmmmmm..." + ";You lost " + AILevel.ToString() + " against " + correctAnswers.ToString() + ",;with " + numberOfQuestions + " questions total..." + ";I really think you;could've done better...", 2));
            }
        }
        yield return new WaitUntil(() => AZAnim.TypeWritingIsFinished);
        yield return new WaitForSeconds(3.5f);
        ReturnToMenu();
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

    //Transitions within the prefab
    void MakeInternalTransition(GameObject toObject, GameObject fromObject)
    {
        toObject.SetActive(true);
        fromObject.SetActive(false);
    }

    //Calculation to convert scrollbar value from 1 to 10

    int GetScrollBarValue(Scrollbar sb)
    {
        int finalCalculation = Mathf.FloorToInt(sb.value * 10);
        if (finalCalculation != 10)
        {
            finalCalculation++;
        }
        return finalCalculation;
    }
    #endregion

    //Public methods for events
    #region EVENTS
    public void LimitAnswerInput()
    {
        string _limitedInput = AZAnim.LimitString(answerField.text, 0, -1, true);
        answerField.text = _limitedInput;
    }

    public void ReturnToMenu()
    {
        //Transition to main menu
        Instantiate(mainMenuPrefab);
        Destroy(gameObject);
    }

    public void PVEButton()
    {
        MakeInternalTransition(PVEModeSettings, modeChoice);
        numberOfQuestionsNumberText = numberOfQuestionsScrollbar.GetComponentInChildren<Text>();
        AILevelNumberText = AILevelScrollbar.GetComponentInChildren<Text>();
        AILevelNumberText.text = GetScrollBarValue(AILevelScrollbar).ToString();
        numberOfQuestionsNumberText.text = GetScrollBarValue(numberOfQuestionsScrollbar).ToString();
    }

    public void PVPButton()
    {
        //MakeInternalTransition(PVPModeSettings, modeChoice);
    }

    public void SubmitButton()
    {
        if (PVEModeSettings.activeSelf == true /*&& PVPModeSettings.activeSelf== true*/) MakeInternalTransition(examination, PVEModeSettings);
        //else MakeInternalTransition(examination, PVPwModeSettings);
        SetRndIntsList();

        //Load all the variables
        correctAnswers = new SecureInt(0);
        AICorrectAnswers = new SecureInt(0);
        numberOfQuestions = GetScrollBarValue(numberOfQuestionsScrollbar);
        AILevel = new SecureFloat(GetScrollBarValue(AILevelScrollbar));

        helperTextAnimator.speed = 0f;

        answerField.ActivateInputField();         //Focus on input field
        Cursor.visible = false;                   //Cursor is not visible
        Cursor.lockState = CursorLockMode.Locked; //Lock the cursor in the middle

        //Coroutine is stored in a variable, with all attributes assigned
        startExaminationCoroutine = StartExamination();
        StartCoroutine(startExaminationCoroutine); //Starts the coroutine
        //Turns text into a form of AICorrectAnswers/Numer of questions
         AICorrectAnswersText.text = "AI: " + AICorrectAnswers.ToString() + "/" + numberOfQuestions;
        //Turns text into a form of CorrectAnswers/Numer of questions
        correctAnswersText.text = correctAnswers.ToString() + "/" + numberOfQuestions;

        timeBar.gameObject.SetActive(false); //To refer to some features of UI element, you need too refer to it's "gameObject" property

        //Clears the helper text
        helperText.text = string.Empty;
    }

    public void NumberOfQuestionsScrollBar()
    {
       numberOfQuestionsNumberText.text = GetScrollBarValue(numberOfQuestionsScrollbar).ToString();
    }

    public void AILevelScrollBar()
    {
        AILevelNumberText.text = GetScrollBarValue(AILevelScrollbar).ToString();
    }
    #endregion
}
