using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopQuizController : MonoBehaviour {

    public Button prevbtn;
    public Button nextbtn;

    private int SceneIndex;
    private string getNextStepID = "http://localhost:81/GetNextStepID.php";

    private void Start()
    {
        if (GlobalVariables.Currentstep == 1)
        {
            prevbtn.interactable = false;
        }

        if (GlobalVariables.CourseLength == GlobalVariables.Currentstep)
        {
            nextbtn.interactable = false;
        }

       // StartCoroutine(LoadQuiz());
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
}
