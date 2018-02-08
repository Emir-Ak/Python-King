using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public static class TypeWriter
{
    public static bool TypingIstFinished;

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
        TypingIstFinished = false;

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

        TypingIstFinished = true;
    }
}

