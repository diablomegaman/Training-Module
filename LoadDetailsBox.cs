using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadDetailsBox : MonoBehaviour {

    private string SegmentDetails = "http://localhost:81/GetDetails.php";
    private string SegmentScore = "http://localhost:81/GetScore.php";
    private string segmentID = "";
    private int numberOfDetails = 2;
    private int numberOfDetailsLoaded = 0;
    private bool detailsLoaded = false;

    public Dropdown SegmentDropdown;
    public GameObject Cover;
    public Text scoreText;
    public Text DetailsText;
    public Text IdBox;

    private void Start()
    {
        SegmentDropdown.onValueChanged.AddListener(delegate { OnDropdownChange(SegmentDropdown); });
        IdBox.text = GlobalVariables.UserID.ToString();
    }

    private void Update()
    {

        if (numberOfDetails == numberOfDetailsLoaded)
        {
            detailsLoaded = true;
        }

        if (detailsLoaded)
        {
            Cover.SetActive(false);
            numberOfDetailsLoaded = 0;
        }

    }

    void Destroy()
    {
        SegmentDropdown.onValueChanged.RemoveAllListeners();
    }

    public void OnDropdownChange(Dropdown drop)
    {

        string segment = drop.options[drop.value].text;

        WWWForm form = new WWWForm();
        form.AddField("segment", segment);
        form.AddField("userID", GlobalVariables.UserID);

        Cover.SetActive(true);
        StartCoroutine(  GetScore(form));
        StartCoroutine(GetDetails(form));

    }

    IEnumerator GetScore(WWWForm f)
    {

        WWW w = new WWW(SegmentScore, f);
        yield return w;

        if (w.error != null)
        {
            print("Could not retreve Score: " + w.error);
        }
        else
        {
            if (w.text == "false")
            {
                scoreText.text = "0";
            }
            else
            {
                scoreText.text = w.text;
            }
        }

        numberOfDetailsLoaded++;
    }

    IEnumerator GetDetails(WWWForm f)
    {

        WWW w = new WWW(SegmentDetails, f);
        yield return w;

        if (w.error != null)
        {
            print("Could not retreve Details: " + w.error);
        }
        else
        {
            DetailsText.text = w.text;
        }

        numberOfDetailsLoaded++;
    }

    public void SetDropdownIndex(int index)
    {
        SegmentDropdown.value = index;
    }

}