using System.IO;
using System.Collections.Generic;

public class QAContainer {
    public static List<string> pythonQuestions = new List<string>();
    public static List<string> pythonSolutions = new List<string>();
    public static List<float> pythonTimings = new List<float>();
 
    //Assigns data from txt files to describe questions (q,a,t)
    public static void ReadData()
    {
        pythonQuestions.AddRange(ReadString("Assets/Prefabs/QuestionStats/PythonQuestions.txt").Split('\n'));
        pythonSolutions.AddRange(ReadString("Assets/Prefabs/QuestionStats/PythonSolutions.txt").Split('\n'));

        //Don't ask, I dont know why... xD
        for (int i = 0; i < pythonSolutions.Count; i++)
        {
            pythonSolutions[i] = pythonSolutions[i].Substring(0, pythonSolutions[i].Length - 1);
        }

        foreach(string line in (ReadString("Assets/Prefabs/QuestionStats/PythonTimings.txt").Split('\n')))
        {
            pythonTimings.Add(float.Parse(line));
        }
    }

    //Returns a read string from txt file
    static string ReadString(string path)
    {
        //Read the text from directly from the txt file
        StreamReader reader = new StreamReader(path);
        return reader.ReadToEnd();
    }
}

