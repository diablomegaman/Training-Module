using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizCompletedController : MonoBehaviour {

    public Text ScoreText;

	// Use this for initialization
	void Start () {
        ScoreText.text = GlobalVariables.QuizScore + "";
	}
	
}
