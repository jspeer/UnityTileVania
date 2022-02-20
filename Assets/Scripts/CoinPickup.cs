using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSfx;
    [SerializeField] int coinValue = 1;
    bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isCollected) {
            isCollected = true;
            // Add coin to coin text
            FindObjectOfType<GameSession>().AddToScore(coinValue);
            // Play sound
            AudioSource.PlayClipAtPoint(coinPickupSfx, Camera.main.transform.position);
            // Disable coin so we can't collect it twice
            gameObject.SetActive(false);
            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
