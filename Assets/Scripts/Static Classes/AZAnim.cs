using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AZAnim
{

    public static bool TypeWritingIsFinished;

    //Description of the static method (can be non-static method description)
    /// <summary>
    /// Creates effect on a text that makes it look like it is being typed in!
    /// </summary>
    /// <param name="text">Text to put effect on</param>
    /// <param name="result">String that is required to be seen as an end result of the text</param>
    /// <param name="time">Time it would take for the effect to take place</param>
    /// <param name="character">Character which won't be typed in, but will put a following text on to a new line</param>
    /// <param name="relative">Would text to take effect on be relative to existing text (or should original text be deleted)?</param>
    /// <param name="delay">Time on seconds before effect will take place</param>

    public static IEnumerator TypeWrite(Text text, string result, float time = 1f, char character = ';', bool relative = false, float delay = 0f)
    {
        #region LOGIC
        TypeWritingIsFinished = false;

        //Delay before typewriting
        yield return new WaitForSeconds(delay);

        //If suggested text is present...
        if (text != null)
        {

            //If suggested resulting string is present...
            if (result != null)
            {
                //Learn the time to spend for each letter
                time = time / result.Length;

                //If text to typewrite is not relative to original... (If it is, characters would be added onto existing text)
                if (relative == false)
                {
                    //Clean the original text
                    text.text = "";
                }

                //For every character in the text required...
                foreach (char i in result)
                {
                    //Will put the following text on to new line
                    if (i == character)
                    {
                        text.text += "\n";
                        continue;
                    }

                    text.text += i;
                    yield return new WaitForSeconds(time);
                }
            }

            else
            {
                Debug.LogError("Suggested result is null! (TypeWrite method)");
            }
        }

        else
        {
            Debug.LogError("Suggested text is null! (TypeWrite method)");
        }

        TypeWritingIsFinished = true;
        #endregion
    }

    /// <summary>
    /// Animates scale of the object
    /// </summary>
    /// <param name="obj">GameObject to put effect on</param>
    /// <param name="framedTime">Time.fixedDeltaTime or Time.deltaTime</param>
    /// <param name="time">Time before scaling stops</param>
    /// <param name="vectorValue">Value that will be passed to additional vector</param>
    /// <param name="delay">Delay in time, before coroutine starts</param>
    /// <returns></returns>
    public static IEnumerator AnimateScale(GameObject obj, float framedTime, float time = 1, float vectorValue = 0.05f, float delay = 0f)
    {
        #region LOGIC
        //Delay before scaling
        yield return new WaitForSeconds(delay);
        //Reasures that the time is set correctly
        if (time > 0f)
        {
            //Counter variable to count the time
            float timeCounter = 0f;

            //Until the curtain time passed...
            while (timeCounter < time)
            {
                //Scale object
                obj.transform.localScale += new Vector3(vectorValue, vectorValue, vectorValue);
                timeCounter += framedTime;             //Add onto counter
                yield return new WaitForFixedUpdate(); //Act as a FixedUpdate function
            }
        }

        else
        {
            Debug.LogError("Time out of range!");
        }
        #endregion
    }
}


