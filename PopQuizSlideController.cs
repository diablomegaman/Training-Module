using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopQuizSlideController : MonoBehaviour {

    public Button prevbtn;
    public Button nextbtn;
    public Text prevBtnText;
    public Text nextBtnText;
    public GameObject[] Toggles = new GameObject[5];
    public Text[] ToggleText = new Text[5];
    public Text QuestionText;
    public GameObject correct;
    public GameObject worng;

    private Question PopQuestion = new Question();
    private int correctToggle;
    private int SceneIndex;
    private int StepID;
    private int QuestionID;
    private string getNextStepID = "http://localhost:81/GetNextStepID.php";
    private string getStepIDURL = "http://localhost:81/GetStepID.php";
    private string getQuestionIDURL = "http://localhost:81/GetQuestionID.php";
    private string getQuestionTextURL = "http://localhost:81/GetQuestionText.php";
    private string getQuestionAnswersURL = "http://localhost:81/GetQuestionAnsers.php";

    void Start()
    {

        if (GlobalVariables.Currentstep == 1)
        {
            prevbtn.interactable = false;
        }

        //if the couse length is equale to the 
        if (GlobalVariables.CourseLength == GlobalVariables.Currentstep)
        {
            nextBtnText.text = "Take Quiz";
        }

        StartCoroutine(LoadQuestion());
    }


    IEnumerator LoadQuestion()
    {
        WWWForm form = new WWWForm();
        form.AddField("stepNumber", GlobalVariables.Currentstep);
        form.AddField("segmentID", GlobalVariables.CurrentSegmentID);

        yield return StartCoroutine(getStepID(form));

        form.AddField("stepID", StepID);

        yield return StartCoroutine(getQuestionID(form));

        form.AddField("questionID", PopQuestion.QuestionID);

        yield return StartCoroutine(getQuestionText(form));
        yield return StartCoroutine(getQuestionAnswers(form));

        LoadSceneText();
    }

    void LoadSceneText()
    {

        QuestionText.text = PopQuestion.QuestionText;

        for (int i = 0; i < PopQuestion.answerText.Length; i++)
        {
            Toggles[i].SetActive(true);
            ToggleText[i].text = PopQuestion.answerText[i].AnswerText;
        }


    }

    IEnumerator getStepID(WWWForm f)
    {
        WWW w = new WWW(getStepIDURL, f);
        yield return w;

        if (w.error != null)
        {
            print("could not get StepID: " + w.error);
        }
        else
        {
            StepID = int.Parse(w.text);
        }
    }

    IEnumerator getQuestionID(WWWForm f)
    {
        WWW w = new WWW(getQuestionIDURL, f);
        yield return w;

        if (w.error != null)
        {
            print("could not get QuestionID: " + w.error);
        }
        else
        {
            PopQuestion.QuestionID = int.Parse(w.text);

        }
    }

    IEnumerator getQuestionText(WWWForm f)
    {
        WWW w = new WWW(getQuestionTextURL, f);
        yield return w;

        if (w.error != null)
        {
            print("could not get QuestionID: " + w.error);
        }
        else
        {
            PopQuestion.QuestionText = w.text;
        }
    }

    IEnumerator getQuestionAnswers(WWWForm f)
    {
        WWW w = new WWW(getQuestionAnswersURL, f);
        yield return w;

        if (w.error != null)
        {
            print("could not get QuestionID: " + w.error);
        }
        else
        {
            LoadAnswers(w.text);
        }
    }

    //Load Answers of the Quiz
    void LoadAnswers(string loadString)
    {
        int start = 0;
        int currentindex = 0;
        int carrotCount = 0;


        for (int stringi = 0; stringi < loadString.Length; stringi++)
        {
            if (loadString[stringi] == '^')
            {

                //To set the start equal to where the carrot is + 1
                if (carrotCount == 0)
                {
                    PopQuestion.answerText = new Answers[int.Parse(loadString.Substring(start, stringi - start))];

                    for (int i = 0; i < PopQuestion.answerText.Length; i++)
                    {
                        PopQuestion.answerText[i] = new Answers();
                    }

                }
                else
                {

                    if (carrotCount == 1)
                    {
                        PopQuestion.answerText[currentindex].AnswerText = loadString.Substring(start, stringi - start);
                    }
                    else if (carrotCount == 2)
                    {
                        if (loadString.Substring(start, stringi - start) == "1")
                        {
                            PopQuestion.CorrectAnswer = currentindex;
                        }

                        currentindex++;
                        carrotCount = 0;
                    }
                }

                carrotCount++;
                stringi++;
                start = stringi;
            }
        }
    }

    public void nextbtn_click()
    {
        GlobalVariables.Currentstep++;
        StartCoroutine(GetNextStep());
    }

    IEnumerator GetNextStep()
    {
        WWWForm form = new WWWForm();
        form.AddField("nextstep", GlobalVariables.Currentstep);
        form.AddField("segmentID", GlobalVariables.CurrentSegmentID);

        WWW w = new WWW(getNextStepID, form);
        yield return w;

        if (w.error != null)
        {
            print("Did not work" + w.error);
        }
        else
        {
            Loadvariables(w.text);
        }
    }

    void Loadvariables(string s)
    {
        int start = 0;
        int carretcount = 0;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '^')
            {

                if (carretcount == 0)
                {
                    GlobalVariables.CurrentstepID = int.Parse(s.Substring(start, i - start));
                    carretcount++;
                }
                else if (carretcount == 1)
                {

                    if (s.Substring(start, i - start) == "1")
                    {
                        SceneIndex = 6;
                    }
                    else if (s.Substring(start, i - start) == "2")
                    {
                        SceneIndex = 5;
                    }
                    else if (s.Substring(start, i - start) == "3")
                    {
                        SceneIndex = 7;
                    }
                }

                i++;
                start = i;
            }
        }

        SceneManager.LoadScene(SceneIndex);
    }

    public void prevbtn_click()
    {
        GlobalVariables.Currentstep--;
        StartCoroutine(GetNextStep());
    }

    //OnToggle1 Answer selected
    public void onToggle1Change()
    {
        //if the CorrectAnswer is equal to the toggle selected
        if (PopQuestion.CorrectAnswer == 0)
        {
            //Display correct text
            correct.SetActive(true);
            //hide Worng text
            worng.SetActive(false);
            //Enable the next Button
            nextbtn.interactable = true;
        }
        else //if The correctAnser is not equal to the toggle selected
        {
            //Hide Correct Text
            correct.SetActive(false);
            //Display Wrong Text
            worng.SetActive(true);
            //Disable the Next Button
            nextbtn.interactable = false;
        }

    }

    public void onToggle2Change()
    {
        if (PopQuestion.CorrectAnswer == 1)
        {
            correct.SetActive(true);
            worng.SetActive(false);

            nextbtn.interactable = true;
        }
        else
        {
            correct.SetActive(false);
            worng.SetActive(true);

            nextbtn.interactable = false;
        }

    }

    public void onToggle3Change()
    {
        if (PopQuestion.CorrectAnswer == 2)
        {
            correct.SetActive(true);
            worng.SetActive(false);

            nextbtn.interactable = true;
        }
        else
        {
            correct.SetActive(false);
            worng.SetActive(true);

            nextbtn.interactable = false;
        }

    }

    public void onToggle4Change()
    {
        if (PopQuestion.CorrectAnswer == 3)
        {
            correct.SetActive(true);
            worng.SetActive(false);

            nextbtn.interactable = true;
        }
        else
        {
            correct.SetActive(false);
            worng.SetActive(true);

            nextbtn.interactable = false;
        }

    }

    public void onToggle5Change()
    {
        if (PopQuestion.CorrectAnswer == 4)
        {
            correct.SetActive(true);
            worng.SetActive(false);

            nextbtn.interactable = true;
        }
        else
        {
            correct.SetActive(false);
            worng.SetActive(true);

            nextbtn.interactable = false;
        }
    }

}
