using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsFinished : MonoBehaviour
{
    private float timer = 3f;
    public GameObject deathCanvas;
    public GameObject deathCanvasButtons;
    public GameObject Player;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timer);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Player"))
        {
            timer -= Time.fixedDeltaTime;

            if (timer < 0)
            {
                deathCanvas.SetActive(true);
                deathCanvasButtons.SetActive(true);
                Player.SetActive(false);
                PlayerPrefs.SetInt("Level1", 1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = 3f;
        }
    }
}

