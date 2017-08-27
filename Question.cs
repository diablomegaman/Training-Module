using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour {

    private int questionID;
    private string questionText;
    private int correctAnswer; 
    private int usersavedAnswer; //toggle index

    public Answers[] answerText;

    public Question()
    {
        questionID = 0;
        questionText = "defult";
        correctAnswer = 0;
        usersavedAnswer = -1;
    }

    public int QuestionID
    {
        get
        {
            return questionID;
        }
        set
        {
            questionID = value;
        }
    }

    public string QuestionText
    {
        get
        {
            return questionText;
        }
        set
        {
            questionText = value;
        }
    }

    public int CorrectAnswer
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

    public int UsersavedAnswer
    {
        get
        {
            return usersavedAnswer;
        }
        set
        {
            usersavedAnswer = value;
        }
    }

    public void setAnswerTextLength(int length)
    {
        answerText = new Answers[length];
    }


}