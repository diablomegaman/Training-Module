using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    private static int userID;
    private static int currentSegmentID;
    private static string userName;
    private static string currentSegmentName;
    private static int currentstep;
    private static int courseLength;
    private static int currentstepID;
    private static float quizScore;

    public static int UserID
    {
        get
        {
            return userID;
        }
        set
        {
            userID = value;
        }
    }

    public static int CurrentSegmentID
    {
        get
        {
            return currentSegmentID;
        }
        set
        {
            currentSegmentID = value;
        }
    }

    public static string UserName
    {
        get
        {
            return userName;
        }
        set
        {
            userName = value;
        }
    }

    public static string CurrentSegmentName
    {
        get
        {
            return currentSegmentName;
        }
        set
        {
            currentSegmentName = value;
        }
    }

    public static int Currentstep
    {
        get
        {
            return currentstep;
        }
        set
        {
            currentstep = value;
        }
    }

    public static int CurrentstepID
    {
        get
        {
            return currentstepID;
        }
        set
        {
            currentstepID = value;
        }
    }

    public static int CourseLength
    {
        get
        {
            return courseLength;
        }
        set
        {
            courseLength = value;
        }
    }

    public static float QuizScore
    {
        get
        {
            return quizScore;
        }
        set
        {
            quizScore = value;
        }
    }
}
