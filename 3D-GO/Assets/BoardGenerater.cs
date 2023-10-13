using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject boardUnitPrefab;  // Drag your "BoardUnit" cube here in the inspector
    public int size = 9;  // For a 9x9x9 grid

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    Instantiate(boardUnitPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}