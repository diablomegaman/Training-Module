using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class VideoController : MonoBehaviour {

    private string url = "http://localhost:81/v/SexualVideo.mp4";
    private string getNextStepID = "http://localhost:81/GetNextStepID.php";
    private string getVideoURL = "http://localhost:81/GetVideoURL.php";

    public UnityEngine.Video.VideoPlayer videoPlayer;
    public Button prevbtn;
    public Button nextbtn;
    private int SceneIndex;

    // Use this for initialization
    void Start () {

        if (GlobalVariables.Currentstep == 1)
        {
            prevbtn.interactable = false;
        }

        if (GlobalVariables.CourseLength == GlobalVariables.Currentstep)
        {
            nextbtn.interactable = false;
        }

        StartCoroutine(LoadVideo());
       //videoPlayer.url = VideoURL;
    }

    IEnumerator LoadVideo()
    {
        WWWForm form = new WWWForm();
        form.AddField("stepID",GlobalVariables.CurrentstepID);

        WWW w = new WWW(getVideoURL, form);
        yield return w;

        if (w.error != null)
        {
            print("could not find URL: " + w.error);
        }
        else
        {
            videoPlayer.url = w.text;
        }
    }

    public void prevbtn_click()
    {
        GlobalVariables.Currentstep--;
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

    public void nextbtn_click()
    {
        GlobalVariables.Currentstep++;
        StartCoroutine(GetNextStep());
    }
}
