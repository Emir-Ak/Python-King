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

    //Time available to examiner for 1 question
    [Tooltip("TIme available for one question")]
    public float timeAvailable;

    [SerializeField]
    InputField answerField;
    [SerializeField]
    Text correctAnswersDisplay;
    int correctAnswers = 0;
    int currentQuestionIndex = 0;
  

    //All variables needed for timing bar (which is a slider)
    [SerializeField]
    Slider timeBar;       //To refer to slider's value
    float timeBarValue;   //Needed in value calculations

    int solutionLength;

    private IEnumerator startExaminationcoroutine;
    #endregion
    #region Start
    private void Start()
    {
        //Starts the coroutine at start (later will be activated by a button)
        startExaminationcoroutine = StartExamination(questions, currentQuestionText, timeAvailable, answerField);
        StartCoroutine(startExaminationcoroutine);
        correctAnswersDisplay.text = correctAnswers.ToString() + "/" + questions.Count.ToString();

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
    IEnumerator StartExamination(List<Question> listOfQuestions, Text spaceForQuestions, float timeForQuestion, InputField answer)
    {
        //If any questions present...
        if (listOfQuestions != null)
        {
            answer.characterValidation = InputField.CharacterValidation.Alphanumeric;

            //For every question in the list
            foreach (Question question in listOfQuestions)
            {
                if (listOfQuestions.IndexOf(question) != currentQuestionIndex)
                {
                    continue;
                }
                answer.characterLimit = question.solution.Length;

                StartCoroutine(TypeWriter.TypeWrite(spaceForQuestions, question.pythonQuestion)); //Type in the question

                bool _didTimeFinish = false;

                yield return new WaitWhile(() => !TypeWriter.TypingIsFinished);   //Wait till the type-in animation is finished

                timeBarValue = timeAvailable;         //Sets the value of slider to one (full value)               
                yield return new WaitForEndOfFrame(); //No state of value changing to one is taking places
                timeBar.gameObject.SetActive(true);   //Slider is visible now

                StartCoroutine(CheckAnswer(answer,question.solution, _didTimeFinish));

                currentQuestionIndex++;

                yield return new WaitForSeconds(timeForQuestion); //Wait before next question

                answer.text = string.Empty;

                _didTimeFinish = true;
   
                timeBar.gameObject.SetActive(false); //Slider is no longer visible after value is 0
            }

            spaceForQuestions.text = string.Empty;
        }

        else
        {
            Debug.LogError("No questions in a list (StartExamination)");
        }
    }

    IEnumerator CheckAnswer(InputField inputField, string solution, bool shouldItStop)
    {
        if (solution == string.Empty)
        {
            Debug.LogError("No solution for this question assigned");
        }

        else
        {
            while (shouldItStop == false) {
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    if (inputField.text == solution)
                    {
                        correctAnswers++;
                    }
                        StopCoroutine(startExaminationcoroutine);
                        StartCoroutine(startExaminationcoroutine);
                        correctAnswersDisplay.text = correctAnswers.ToString() + "/" + questions.Count.ToString();
                        break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
    #endregion  
}
