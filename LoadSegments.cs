using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSegments : MonoBehaviour {
    private string loadSegmentDropDownURL = "http://localhost:81/Segments.php";

    public Dropdown SegDropDown;
    public Text DropDownText;
    public Text usertext;

	// Use this for initialization
	void Start ()
    {
        usertext.text = GlobalVariables.UserName;
        StartCoroutine(loadSegments());
	}

    ///////////////////////
    //Connection Function//
    ///////////////////////
    IEnumerator loadSegments()
    {
        DropDownText.text = "Loading...";
        WWW text_get = new WWW(loadSegmentDropDownURL);
        yield return text_get;

        if (text_get.error != null)
        {
            print("Could not load Segments: " + text_get.error);
        }
        else
        {
            InsertDropDown(text_get.text);
            DropDownText.text = "Select Segment";
            SegDropDown.interactable = true;
        }
    }

    ////////////////////////
    ///load ComboBox code///
    ////////////////////////
    //The User can not use a '^' in there data or the 
    //segments will break the string correctly.
    void InsertDropDown(string loadString)
    {
        int start = 0;

        for (int i = 0; i < loadString.Length; i++)
        {
            if (loadString[i] == '^')
            {
                Dropdown.OptionData data = new Dropdown.OptionData(loadString.Substring(start, i - start));
                SegDropDown.options.Add(data);

                //To set the start equal to where the carrot is + 1
                i++;
                start = i;
            }
        }
    }
}