using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CompletedController : MonoBehaviour
{

    public Text ScoreText;
    public GameObject Goodjob;
    public GameObject Tryagian;
    public GameObject failed;
    public GameObject passed;
    public Text test;

    private string SetScoreURL = "http://localhost:81/SetScore.php";

    // Use this for initialization
    void Start()
    {
        if (GlobalVariables.QuizScore < 70)
        {
            Goodjob.SetActive(false);
            Tryagian.SetActive(true);

            failed.SetActive(true);
            passed.SetActive(false);
        }
        else
        {
            Goodjob.SetActive(true);
            Tryagian.SetActive(false);

            failed.SetActive(false);
            passed.SetActive(true);
        }

        ScoreText.text = GlobalVariables.QuizScore + "";
        StartCoroutine(SetScore());
    }

    IEnumerator SetScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("score", GlobalVariables.QuizScore.ToString());
        form.AddField("userID", GlobalVariables.UserID);
        form.AddField("segmentID", GlobalVariables.CurrentSegmentID);

        WWW w = new WWW(SetScoreURL,form);
        yield return w;

        if (w.error != null)
        {
            print("Could not set Score" + w.error);
        }
        else
        {
            Debug.Log(w.text);
        }
    }

    public void CourseSelect_Click()
    {
        GlobalVariables.CurrentSegmentID = 0;
        GlobalVariables.CurrentSegmentName = "";
        SceneManager.LoadScene(1);
    }

    public void Logout_Click()
    {
        GlobalVariables.UserID = 0;
        GlobalVariables.UserName = "";
        GlobalVariables.CurrentSegmentID = 0;
        GlobalVariables.CurrentSegmentName = "";
        SceneManager.LoadScene(0);
    }

}
