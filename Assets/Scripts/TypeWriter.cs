using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TypeWriter : MonoBehaviour {

    public float delay;
    [Tooltip("total time it takes to typewrite, including \";\" and \"&\", BUT excluding time before next string")]
    public float time;
    [Header("Can not be left as null")]
    public string endResult;

    public float timeBeforeNextString;

    [Header("Can be left as null, will get \"Text\" component")]
    [Tooltip("Seperate 2 or more strings by putting \";\" between them, and & is equivalent to \"\\n\"")]
    public Text textToTypeWrite;
    /*
    (Time before new string = 2 sec, overall time 10 secs, not Relative
    Example: Hi, my name is&Emir Akbashev&This is the TypeWriterScript;I was wondering why&cookies are so tasty sometimes...
    Equivalent:
    "Hi, my name is
    Emir Akbashev
    This is the TypeWriterScript
    "
    *Waits for 2 secs*
    *Deletes previous text till the non-relevant point*
    
    "I was wondering why
    cookies are so tasty sometimes..."
    Average time taken: 12 secs
    */


    [Tooltip("Should text be add onto existing text? (Or rewritten if false)")]
    public bool isRelative;

    private string originalText;

    [Header("Coming soon...")]
    public bool isBeforeOriginalText;

    int replayCheck = 0;

    void Start() {
        if (textToTypeWrite == null)
        {
            textToTypeWrite = GetComponent<Text>();
        }

        StartCoroutine(TypeWrite());
    }
  
    private void Update()
    {
        if (textToTypeWrite.enabled == false)
        {
            replayCheck = 1;
        }

        if (textToTypeWrite.enabled == true && replayCheck == 1)
        {
            if (isRelative == true)
            {
                textToTypeWrite.text = originalText;
            }

            StartCoroutine(TypeWrite());
            replayCheck = 0;
        }
      } 

    IEnumerator TypeWrite()
    {

        if (endResult.Contains(";"))
        {
            originalText = textToTypeWrite.text;
        }

        yield return new WaitForSeconds(delay);

        if (endResult != null)
        {
            time = time / endResult.Length;

            if (isRelative == false)
            {
                textToTypeWrite.text = "";
            }

            foreach (char i in endResult)
            {
                if (i == '&')
                {
                    textToTypeWrite.text += "\n";
                    continue;
                }
                if (i == ';')
                {
                    yield return new WaitForSeconds(timeBeforeNextString);
                    if(isRelative == false) textToTypeWrite.text = "";
                    else textToTypeWrite.text = originalText;

                    continue;
                }

                textToTypeWrite.text += i;
                yield return new WaitForSeconds(time);
            }
        }

        else
        {
            Debug.LogError("Suggested string is null!");
        }
    }
}
