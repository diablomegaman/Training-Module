using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
//using UnityEditor;

public class LoginController : MonoBehaviour {

    private string CheckLoginURL = "http://localhost:81/CheckLogin.php";
    private string RetreveUserIDURL = "http://localhost:81/GetUserID.php";
    private bool readyToSwitch = false;
    private int NumOfPointsToReach = 2;
    private int pointsReached = 0;

    public InputField username;
    public InputField password;
    public GameObject Cover;

    public GameObject MessageBox;
    public Text MessageHeader;
    public Text MessageText;

    public void Update()
    {
        if (NumOfPointsToReach == pointsReached)
        {
            readyToSwitch = true;
        }

        if (!MessageBox.activeInHierarchy)
        {
            if (readyToSwitch)
            {
                SceneManager.LoadScene(1);
            }
        }

    }

    public void LoginIn_Clicked(){
        WWWForm Form = new WWWForm();
        Form.AddField("username", username.text);
        Form.AddField("password", password.text);

        StartCoroutine(Authenticating(Form));
    }

    IEnumerator Authenticating(WWWForm www)
    {

        Cover.SetActive(true);
        WWW CheckLogin = new WWW(CheckLoginURL,www);
        yield return CheckLogin;
        
        if (CheckLogin.error != null)
        {
            print("Could not Log in: " + CheckLogin.error);
        }
        else
        {

            if (CheckLogin.text == "true")
            {

                StartCoroutine(StoreUserId(www));
                GlobalVariables.UserName = username.text;

                //Display message box For user
                MessageBox.SetActive(true);
                MessageHeader.text = "Login Response";
                MessageText.text = "Login Successful";

                //Makes the Scene ready to switch
                pointsReached++;
            }
            else if (CheckLogin.text == "false")
            {
                //Display message box For user
                MessageBox.SetActive(true);
                MessageHeader.text = "Login Response";
                MessageText.text = "Username or Password was incorrect: Please Try again";

                Cover.SetActive(false);
            }
            else
            {

                //Display message box For user
                MessageBox.SetActive(true);
                MessageHeader.text = "Login Response";
                MessageText.text = "Login Failed: unknown error";

                Cover.SetActive(false);
            }
        }
    }

    IEnumerator StoreUserId(WWWForm f)
    {
        WWW w = new WWW(RetreveUserIDURL,f);
        yield return w;

        if (w.error != null || w.text == "false")
        {
            print("Could not find Username: "+ w.error);    
        }
        else
        {
            GlobalVariables.UserID = int.Parse(w.text);
        }
        pointsReached++;
    }

    public void Message_Click()
    {
        MessageBox.SetActive(false);
    }

}
