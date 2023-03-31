using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class chooseEpisodeButtons : MonoBehaviour
{
    public Button firstButton;
    public Button secondButton;
    public Button goBackButton;
    private string firstText = "1";
    private string secondText = "2";
    private string goBack = "";
    public GameObject level1FinishedFlag;

    void Start()
    {
        // Get the Text component attached to the button
        Text button1Text = firstButton.GetComponentInChildren<Text>();
        Text button2Text = secondButton.GetComponentInChildren<Text>();
        Text buttonGoBack = goBackButton.GetComponentInChildren<Text>();
        // Change the text of the button
        button1Text.text = firstText;
        button2Text.text = secondText;
        buttonGoBack.text = goBack;

        // Check if the user has completed level 2
        if (PlayerPrefs.GetInt("Level1", 0) == 1)
        {
            level1FinishedFlag.SetActive(true);
        }


    }


    public void LoadEpisode1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadEpisode2()
    {
        SceneManager.LoadScene("secondEpisode");
    }

    public void GoBackToEntry()
    {
        SceneManager.LoadScene("OpeningScene");
    }


}
