using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

// AUTHOR: Mitchell basile
// Description: Loads the Quiz box for dynamic use from a database.
//              gets all questions stores them into testQuestions class array
//              then with in that class there is an answers array that stores 
//              all of the answers for the question.
//
public class QuizController : MonoBehaviour
{

    public GameObject Content;
    public Text TestQuestionText;
    public GameObject PrevBtn;
    public GameObject NextBtn;
    public Text nextBtnText;
    public Text QuestionNumber;
    public InputField currentinputbox;
    public InputField totalinputbox;
    public GameObject[] Toggles = new GameObject[5];
    public Text[] toggletext = new Text[5];
    private int correctToggle;

    //public GameObject Toggle; prefab of toggle for createing of the moduler answers later.

    private Question[] testQuestions;
    private string numOfQuestionsURL = "http://localhost:81/GetNumOfQuestions.php";
    private string questionInfoURL = "http://localhost:81/GetQuestionInfo.php";
    private string AnswersInfoURL = "http://localhost:81/GetAnswerInfo.php";

    //private GameObject[] toggles; used for moduler answers later

    private bool loadQuiz = false;
    private int currentQuestion = 0;

    // Use this for initialization
    void Start () {
        loadingQuestions();
	}

    private void Update()
    {
        if (loadQuiz)
        {
            // load Quiz box
            LoadQuizBox();
            loadQuiz = false;
        }
    }

    /*
        After the Arrays are loaded This function Simply loads the 
        Text into the Quiz Segment
    */
    void LoadQuizBox()
    {
        //Turns off all answers
        //so we can only turn on what we need later
        for (int i = 0; i < Toggles.Length; i++)
        {
            Toggles[i].GetComponent<Toggle>().isOn = false;
            Toggles[i].SetActive(false);
        }

        //set QuestionNumber and QuestionText
        QuestionNumber.text = (currentQuestion + 1) + ")";
        TestQuestionText.text = testQuestions[currentQuestion].QuestionText;

        //Turn on Each answer based on the number of answers
        for (int i = 0; i < testQuestions[currentQuestion].answerText.Length; i++)
        {
            Toggles[i].SetActive(true);
            toggletext[i].text = testQuestions[currentQuestion].answerText[i].AnswerText;
        }

        //If the user was prevously on this question The answer was saved
        //set the Toggle to the saved answer
        if (testQuestions[currentQuestion].UsersavedAnswer != -1)
        {
            Toggles[testQuestions[currentQuestion].UsersavedAnswer].GetComponent<Toggle>().isOn = true;
        }

        //code used for moduler answers are at bottom of code.
    }


    //Starts the Data retreval from the database
    void loadingQuestions()
    {
        StartCoroutine(SetQuestion());
    }


    IEnumerator SetQuestionText()
    {
        WWWForm form = new WWWForm();
        form.AddField("segment", GlobalVariables.CurrentSegmentID);

        WWW w = new WWW(questionInfoURL, form);
        yield return w;

        if (w.error != null)
        {
            print("could not return number of questions: " + w.error);
        }
        else
        {
            LoadQuestionInfo(w.text);
            StartCoroutine(setAnswers());
        }
    }

    //For each question call GetAnswerInfo
    IEnumerator setAnswers()
    {
        for (int i = 0; i < testQuestions.Length; i++)
        {
            yield return StartCoroutine(GetAnswerInfo(testQuestions[i].QuestionID,i));
        }
        //Once all the answers are loaded Set loadQuiz to True;
        loadQuiz = true;
    }

    //Calls the php used on the server to retreve
    //The answers for the questions inside of the database
    IEnumerator GetAnswerInfo(int ID,int index)
    {
        //Storse the question ID to retreve the question answers
        WWWForm form = new WWWForm();
        form.AddField("ID",ID);

        //Executes the php
        WWW w = new WWW(AnswersInfoURL, form);
        yield return w;

        //Checks for an error
        if (w.error != null)
        {
            print("could not return number of questions: " + w.error);
        }
        else
        {
            //put answers into the array
            LoadAnswers(w.text,index);
        }

    }

    //takes the information from the string and loads the information into array
    //we need to parse the string and send the information into the proper element
    void LoadAnswers(string loadString,int QuestionArrayIndex)
    {
        int start = 0;          //where to start a substring
        int currentindex = 0;   //where we are currently in the array of answers
        int CarratCount = 0;    //How many carrats we have ran into - carrat is the dalimitor

        //for each character in the string
        for (int stringi = 0; stringi < loadString.Length; stringi++)
        {
            //check to see if it is a carrat
            if (loadString[stringi] == '^')
            {
                //increase carratCount
                CarratCount++;

                //To set the start equal to where the carrot is + 1
                //+1 is so we do not capy the carrat as well
                if (CarratCount == 1)
                {
                    //set the answer ID
                    testQuestions[QuestionArrayIndex].answerText[currentindex].AnswerID = int.Parse(loadString.Substring(start, stringi - start));
                }
                else if (CarratCount == 2)
                {
                    //Set the answer Text
                    testQuestions[QuestionArrayIndex].answerText[currentindex].AnswerText = loadString.Substring(start, stringi - start);
                }
                else
                {
                    //Set if the answer is the corret answer or not
                    if (loadString.Substring(start, stringi - start) == "0")
                    {
                        testQuestions[QuestionArrayIndex].answerText[currentindex].CorrectAnswer = false;
                    }
                    else if (loadString.Substring(start, stringi - start) == "1")
                    {
                        testQuestions[QuestionArrayIndex].answerText[currentindex].CorrectAnswer = true;
                        testQuestions[QuestionArrayIndex].CorrectAnswer = testQuestions[QuestionArrayIndex].answerText[currentindex].AnswerID;
                    }

                    //reset the carratCount 
                    //to loop though the information needed again    
                    CarratCount = 0;
                    currentindex++;
                }

                //Skip over the carrat and start at another character
                stringi++;
                start = stringi;
            }
        }
    }


    void LoadQuestionInfo(string loadString)
    {
        int start = 0;
        int currentindex = 0;
        int CarratCount = 0;

        //Load
        for (int i = 0; i < loadString.Length; i++)
        {
            if (loadString[i] == '^')
            {
                //Controls what feild the information goes into
                CarratCount++;

                //To set the start equal to where the carrot is + 1
                if (CarratCount == 1) // QuestionID
                {
                    testQuestions[currentindex].QuestionID = int.Parse(loadString.Substring(start, i - start));
                }
                else if (CarratCount == 2) // Question Text
                {
                    testQuestions[currentindex].QuestionText = loadString.Substring(start, i - start);
                }
                else //Answer text size
                {
                    testQuestions[currentindex].answerText = new Answers[int.Parse(loadString.Substring(start, i - start))];

                    //initualize Array of answers
                    for (int x = 0; x < testQuestions[currentindex].answerText.Length; x++)
                    {
                        testQuestions[currentindex].answerText[x] = new Answers();
                    }

                    CarratCount = 0;
                    currentindex++;
                }

                i++;
                start = i;
            }
        }
    }


    //Retreves the data for the questions from the database
    IEnumerator SetQuestion()
    {
        WWWForm form = new WWWForm();
        form.AddField("segment",GlobalVariables.CurrentSegmentID);

        WWW w = new WWW(numOfQuestionsURL, form);
        yield return w;

        if (w.error != null)
        {
            print("could not return number of questions: " + w.error);
        }
        else
        {
            Debug.Log(w.text);
            setTestQuestion(w.text);   
            StartCoroutine(SetQuestionText());
        }

    }


    void setTestQuestion(string loadstring)
    {
        testQuestions = new Question[int.Parse(loadstring)];
        totalinputbox.text = loadstring;

        for (int i = 0; i < testQuestions.Length; i++)
        {
            testQuestions[i] = new Question();
        }
    }


    public void OpenQuizCompleteScrene()
    {
        SceneManager.LoadScene(4);
    }


    public void Prev_Click()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            if (Toggles[i].GetComponent<Toggle>().isOn)
            {
                testQuestions[currentQuestion].UsersavedAnswer = i;
            }
        }


        currentQuestion--;

        if (currentQuestion == 0)
        {
            PrevBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            PrevBtn.GetComponent<Button>().interactable = true;
            NextBtn.GetComponent<Button>().interactable = true;
            nextBtnText.text = "Next";
        }

        currentinputbox.text = currentQuestion + 1 + "";

        LoadQuizBox();
    }


    public void Next_CLick()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            if (Toggles[i].GetComponent<Toggle>().isOn)
            {
                testQuestions[currentQuestion].UsersavedAnswer = i;
            }
        }

        currentQuestion++;

        if (currentQuestion == testQuestions.Length - 1)
        {
            nextBtnText.text = "Finished";
            //NextBtn.GetComponent<Button>().interactable = false;
        }
        else if (currentQuestion == testQuestions.Length)
        {
            ComputeScore();
            SceneManager.LoadScene(4);
        }
        else
        {
            NextBtn.GetComponent<Button>().interactable = true;
            PrevBtn.GetComponent<Button>().interactable = true;
        }

        currentinputbox.text = currentQuestion + 1 + "";

        LoadQuizBox();
    }

    //Loads the Users score into QuizScore for use on other frames.
    void ComputeScore()
    {
        float count = 0;

        for (int i = 0; i < testQuestions.Length ; i++)
        {
            if (testQuestions[i].UsersavedAnswer > -1)
            {
                if (testQuestions[i].answerText[testQuestions[i].UsersavedAnswer].CorrectAnswer)
                {
                    count++;
                }
            }
        }

        GlobalVariables.QuizScore = (count / testQuestions.Length)*100;

    }
}

        //Code for used for moduler answers.
        /*
        toggles = new GameObject[testQuestions[currentQuestion].answerText.Length];
        for (int i = 0; i < testQuestions[currentQuestion].answerText.Length; i++)
        {
            toggles[i] = Instantiate(Toggle,Content.transform);
            toggles[i].transform.parent = Content.transform;
            Canvas.ForceUpdateCanvases();
            toggles[i].transform.Find("Label").GetComponent<Text>().text = testQuestions[currentQuestion].answerText[i].AnswerText;

            
             * This is where the code to control answer placement goes
             * having trouble laneing them up
            if (i == 0)
            {
                float ToggleX = Content.transform.FindChild("TestQuestion").transform.localPosition.x;
                float ToggleY =  Content.GetComponent<RectTransform>().rect.height - QuestionText.GetComponent<RectTransform>().rect.height; //Content.GetComponent<RectTransform>().localPosition.y;

                Debug.Log(Content.GetComponent<RectTransform>().localPosition.x);
                Debug.Log(Content.GetComponent<RectTransform>().rect.height);
                Debug.Log(ToggleY);

                toggles[i].GetComponent<RectTransform>().transform.localPosition = new Vector3(ToggleX, ToggleY, 0);
            }
            else
            {
                //toggles[i].transform.po
            }
            */