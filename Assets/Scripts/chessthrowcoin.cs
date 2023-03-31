using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chessthrowcoin : MonoBehaviour
{
    public GameObject coinPrefab;

    private bool hasThrownCoin = false;
    private float timer = 3f;
    public float throwForce = 2f;
    public float gravityScaleIncreaseRate = 0.3f;


    private void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        
        if (collision.CompareTag("Player") && !hasThrownCoin)
        {
            timer -= Time.deltaTime;
            if (timer < 0) { 
            GameObject coin = Instantiate(coinPrefab, transform.position + new Vector3(0f,3f,0f), Quaternion.identity);
                Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
                coinRb.AddForce(Vector2.up * throwForce, ForceMode2D.Impulse);
                hasThrownCoin = true;
                StartCoroutine(IncreaseGravityScale(coinRb));
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
    private IEnumerator IncreaseGravityScale(Rigidbody2D rb)
    {
        while (rb.gravityScale < 1f)
        {
            rb.gravityScale += gravityScaleIncreaseRate * Time.deltaTime;
            yield return null;
        }
    }
}
