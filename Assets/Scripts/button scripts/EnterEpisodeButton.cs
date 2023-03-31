using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterEpisodeButton : MonoBehaviour
{

    public Button myButton;
    private string newText = "Start";

    void Start()
    {
        // Get the Text component attached to the button
        Text buttonText = myButton.GetComponentInChildren<Text>();

        // Change the text of the button
        buttonText.text = newText;
    }
    public void LoadEpisodeScene()
    {
        SceneManager.LoadScene("Episodes");
    }
}
