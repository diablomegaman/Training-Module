using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SlideController : MonoBehaviour {

    public RawImage Image;
    public Button prevbtn;
    public Button nextbtn;
    public Text nextBtnText;

    private int SceneIndex;
    private Texture2D tex;
    private string getNextStepID = "http://localhost:81/GetNextStepID.php";
    private string getSlideImageURL = "http://localhost:81/getSlideImage.php"; 

    private void Start()
    {
        if (GlobalVariables.Currentstep == 1)
        {
            prevbtn.interactable = false;
        }

        if (GlobalVariables.CourseLength == GlobalVariables.Currentstep)
        {
            nextBtnText.text = "Take Quiz";
        }

        StartCoroutine(LoadImage());
    }

    IEnumerator LoadImage()
    {

        tex = new Texture2D(16, 16, TextureFormat.RGB24, false);

        WWWForm form = new WWWForm();
        form.AddField("stepID",GlobalVariables.CurrentstepID);

        WWW w = new WWW(getSlideImageURL ,form);

        yield return w;

        WWW image = new WWW(w.text);

        yield return image;

        if (w.error != null)
        {
            print("pic could not load" + w.error);
        }
        else
        {
            image.LoadImageIntoTexture(tex);
            Image.texture = tex;
        }

    }

    public void nextbtn_click()
    {
        if (GlobalVariables.CourseLength == GlobalVariables.Currentstep)
        {
            nextBtnText.text = "Take Quiz";
            SceneManager.LoadScene(3);
        }

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
