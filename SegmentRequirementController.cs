using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SegmentRequirementController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OpenVideoScene()
    {
        SceneManager.LoadScene(5);
    }

    public void OpenDocumentScene()
    {
        SceneManager.LoadScene(6);
    }

    public void OpenQuizScene()
    {
        SceneManager.LoadScene(3);
    }
}
