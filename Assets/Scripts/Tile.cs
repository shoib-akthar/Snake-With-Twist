using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public event Action<Tile> OnStateChanged; // Event to notify observers
    private bool isGreen = false; // Current state of the tile
    public bool IsGreen => isGreen; // Public getter for the state

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!GameManager.Isgridinitialized)
        return;

        if (collision.CompareTag("Snake"))
        {
            ChangeToGreen();
        }
    }

    public void ChangeToGreen()
    {
        if (!isGreen)
        {
            isGreen = true;
            spriteRenderer.color = Color.green;
            spriteRenderer.sortingOrder = -1; // Set the sorting order to 1
            OnStateChanged?.Invoke(this); // Notify observers
        }
    }
}
