using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public Button myButton;
    private string newText = "QUIT";

    void Start()
    {
        // Get the Text component attached to the button
        Text buttonText = myButton.GetComponentInChildren<Text>();

        // Change the text of the button
        buttonText.text = newText;
    }

    public void Quit()
    {
        Application.Quit();
        PlayerPrefs.DeleteKey("Level1");
    }
}
