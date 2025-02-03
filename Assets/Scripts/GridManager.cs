using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 10;
    [SerializeField] private int columns = 10;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private Transform cameraTransform;

    private Tile[,] gridTiles; 

    void Start()
    {

        gridTiles = new Tile[columns, rows];

        cameraTransform.position = new Vector3((columns - 1) * cellSize / 2, (rows - 1) * cellSize / 2, -10);

        GenerateGrid();
        CreateBoundary();
    }

    void GenerateGrid()
    {
        // Clear any previous grid cells
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);

                Tile tile = cell.GetComponent<Tile>();
                if (tile != null)
                {
                    gridTiles[x, y] = tile;
                }
            }
        }
    }

    void CreateBoundary()
    {
        float gridWidth = columns * cellSize;
        float gridHeight = rows * cellSize;

        InstantiateWall(new Vector2(gridWidth / 2 - cellSize / 2, gridHeight), new Vector2(gridWidth, cellSize)); // Top wall
        InstantiateWall(new Vector2(gridWidth / 2 - cellSize / 2, -cellSize), new Vector2(gridWidth, cellSize)); // Bottom wall
        InstantiateWall(new Vector2(-cellSize, gridHeight / 2 - cellSize / 2), new Vector2(cellSize, gridHeight)); // Left wall
        InstantiateWall(new Vector2(gridWidth, gridHeight / 2 - cellSize / 2), new Vector2(cellSize, gridHeight)); // Right wall
    }

    void InstantiateWall(Vector2 position, Vector2 size)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.localScale = new Vector3(size.x, size.y, 1);
    }

    public Tile[] GetAllTiles()
    {
        return transform.GetComponentsInChildren<Tile>();
    }

    public Vector3 GetRandomGridPosition(Vector2 edgeOffset)
    {
        float randomX = Random.Range(edgeOffset.x, (columns - 1) * cellSize - edgeOffset.x);
        float randomY = Random.Range(edgeOffset.y, (rows - 1) * cellSize - edgeOffset.y);
        return new Vector3(randomX, randomY, 0);
    }

}
