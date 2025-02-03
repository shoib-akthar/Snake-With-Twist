using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float foodSpawnInterval = 5f;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private Vector2 edgeOffset = new Vector2(1, 1);
    private GameObject currentFood;
    private int greenTileCount = 0;
    [SerializeField] private Tile[] tiles;
    private bool isPaused = false;
    private bool isGameOver = false;
    private float timer;

    public static event Action OnGameOver;
    public static bool Isgridinitialized = false;


    void Start()
    {
        StartCoroutine(Init());

        Food.OnFoodCollected += HandleFoodCollected;

        currentFood = Instantiate(foodPrefab);
        currentFood.SetActive(true);

        timer = foodSpawnInterval;
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
        tiles = null;
        tiles = gridManager.GetAllTiles();

        foreach (var tile in tiles)
        {
            tile.OnStateChanged += HandleTileStateChanged;
        }

        Isgridinitialized = true;
    }

    void Update()
    {
        if (isGameOver || isPaused) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnFood();
            timer = foodSpawnInterval;
        }
    }

    private void HandleTileStateChanged(Tile tile)
    {
        if (tile.IsGreen)
        {
            greenTileCount++;
            CheckGameOver();
        }
    }

    private void CheckGameOver()
    {
        if (greenTileCount == tiles.Length)
        {
            isGameOver = true;
            OnGameOver?.Invoke();

            if (currentFood != null)
            {
                currentFood.SetActive(false);
            }
        }
    }

    private void HandleFoodCollected()
    {

        if (currentFood != null)
        {
            currentFood.SetActive(false);
        }
    }

    private void SpawnFood()
    {
        DestroyExtraFood();

        Vector3 randomPosition = gridManager.GetRandomGridPosition(edgeOffset);

        currentFood.transform.position = randomPosition;
        currentFood.SetActive(true);
    }

    private void DestroyExtraFood()
    {
        foreach (var food in GameObject.FindGameObjectsWithTag("Food"))
        {
            if (food != currentFood)
            {
                Destroy(food);
            }
        }
    }
    private void OnDisable()
    {
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                tile.OnStateChanged -= HandleTileStateChanged;
            }
        }
        Food.OnFoodCollected -= HandleFoodCollected; // Unsubscribe to prevent memory leaks
    }

    private void RestartGame()
    {
        StartCoroutine(Init());
    }
}
