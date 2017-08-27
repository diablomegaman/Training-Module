using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreen : MonoBehaviour {

    public GameObject loading;
    public Text test;
    public Dropdown SegmentDropdown;

    private int SceneIndex;
    private string FirstStepURL = "http://localhost:81/FirstStep.php";
    private string GetSegmentIDURL = "http://localhost:81/GetSegmentID.php";
    private string getSegmentLengthURL = "http://localhost:81/getSegmentLength.php";

    public void Load()
    {

        loading.SetActive(true);

        StartCoroutine(ExicuteCoroutines());
    }

    IEnumerator GetSegmentID(WWWForm f)
    {

        WWW w = new WWW(GetSegmentIDURL, f);
        yield return w;

        if (w.error != null)
        {
            print("Could not get segment ID" + w.error);
        }
        else
        {
            GlobalVariables.CurrentSegmentID = int.Parse(w.text);
        }
    }

    IEnumerator GetSegmentLength(WWWForm f)
    {

        WWW w = new WWW(getSegmentLengthURL,f);
        yield return w;

        if (w.error != null)
        {
            print("Could not get segment length " + w.error);
        }
        else
        {
            GlobalVariables.CourseLength = int.Parse(w.text);
        }
    }

    IEnumerator ExicuteCoroutines()
    {
        loading.SetActive(true);

        WWWForm formSeg = new WWWForm();
        formSeg.AddField("segmentname", SegmentDropdown.options[SegmentDropdown.value].text);
        yield return StartCoroutine(GetSegmentID(formSeg));

        WWWForm formSegID = new WWWForm();
        formSegID.AddField("segmentID", GlobalVariables.CurrentSegmentID);

        yield return StartCoroutine(GetSegmentLength(formSegID));

        WWWForm form = new WWWForm();
        form.AddField("segment",GlobalVariables.CurrentSegmentID);

        yield return StartCoroutine(FirstStep(form));
    }

    IEnumerator FirstStep(WWWForm f)
    {
        WWW w = new WWW(FirstStepURL, f);
        yield return w;

        if (w.error != null)
        {
            print("Did not work" + w.error);
            loading.SetActive(false);
        }
        else
        {
            GlobalVariables.Currentstep = 1;
            LoadVariables(w.text);
        }

        loading.SetActive(false);
        SceneManager.LoadScene(SceneIndex);
    }

    void LoadVariables(string s)
    {

        int start = 0;
        int carretcount = 0;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '^')
            {
                if (carretcount == 0)
                {
                    GlobalVariables.CurrentstepID = int.Parse(s.Substring(start, i));
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
    }

}
