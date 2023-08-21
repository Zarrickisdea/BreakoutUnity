using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Grid grid;

    private List<GameObject> bricks;

    public static BrickManager Instance { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateBricks(int levelIndex)
    {
        bricks = new List<GameObject>();
        grid.transform.position = new Vector3(grid.transform.position.x, 0f, 0f);
        int maxRows = Random.Range(1, 11);

        int initialColumn = 5;
        int maxColumns = Random.Range(initialColumn, 8);
        maxColumns = maxColumns % 2 == 0 ? maxColumns + 1 : maxColumns;

        int highestPossibleColorIndex = levelIndex < 3 ? 2 : levelIndex;
        int highestColorIndex = Mathf.Min(7, (levelIndex % 7) + highestPossibleColorIndex);
        int lowestColorIndex = levelIndex < 3 ? 0 : Mathf.Min(7, levelIndex - 3);

        float multiplier = maxColumns - initialColumn;
        float yCellGap = 1.1f - (multiplier * 0.3f);
        grid.cellGap = new Vector3(grid.cellGap.x, yCellGap, grid.cellGap.z);

        for (int i = 0; i < maxRows; i++)
        {
            bool skipPatternRow = Random.Range(0, 2) == 0;
            bool skipBrick = Random.Range(0, 2) == 0;
            bool alternateRow = Random.Range(0, 2) == 0;
            bool alternateBrick = Random.Range(0, 2) == 0;

            int mainIndex = BrickColors.GetBrickColor(Random.Range(lowestColorIndex, highestColorIndex)).Item2;

            int altIndex1 = BrickColors.GetBrickColor(Random.Range(lowestColorIndex, highestColorIndex)).Item2;
            int altIndex2 = BrickColors.GetBrickColor(Random.Range(lowestColorIndex, highestColorIndex)).Item2;

            for (int j = 0; j < maxColumns; j++)
            {
                if (skipPatternRow && skipBrick)
                {
                    skipBrick = !skipBrick;
                    continue;
                }
                else
                {
                    skipBrick = !skipBrick;
                }
                Vector3Int cellPosition = new Vector3Int(i, j, 0);
                Vector3 cellCenterPosition = grid.GetCellCenterWorld(cellPosition);
                GameObject brick = Instantiate(brickPrefab, cellCenterPosition, Quaternion.identity);
                brick.transform.SetParent(grid.transform, true);

                Brick brickScript = brick.GetComponent<Brick>();

                if (alternateRow && alternateBrick)
                {
                    brickScript.ColorIndex = altIndex1;
                    alternateBrick = !alternateBrick;
                }
                else if (alternateRow)
                {
                    brickScript.ColorIndex = altIndex2;
                    alternateBrick = !alternateBrick;
                }
                else
                {
                    brickScript.ColorIndex = mainIndex;
                }

                bricks.Add(brick);
            }
        }
    }

    public void CheckBricks()
    {
        if (bricks.Count == 0)
        {
            LevelManager.Instance.LevelIndex += 1;
            LevelManager.Instance.LoadLevel(LevelManager.Instance.LevelIndex);
        }
    }

    public void RemoveBrick(Brick brick)
    {
        bricks.Remove(brick.gameObject);
        Destroy(brick.gameObject);
    }

    public void MoveBricks()
    {
        grid.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y - 1.1f, grid.transform.position.z);
    }
}