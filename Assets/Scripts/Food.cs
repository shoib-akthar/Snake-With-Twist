using System;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static event Action OnFoodCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Snake"))
        {
            OnFoodCollected?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
