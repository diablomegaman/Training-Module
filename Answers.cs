using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answers : MonoBehaviour {

    private int answerID;
    private string answerText;
    private bool correctAnswer;

    public int AnswerID
    {
        get
        {
            return answerID;
        }
        set
        {
            answerID = value;
        }
    }

    public string AnswerText
    {
        get
        {
            return answerText;
        }
        set
        {
            answerText = value;
        }
    }

    public bool CorrectAnswer
    {
        get
        {
            return correctAnswer;
        }
        set
        {
            correctAnswer = value;
        }
    }

}
