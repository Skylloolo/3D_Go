using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum PlayerTurn { Black, White }
    public PlayerTurn currentTurn = PlayerTurn.Black;

    // Assuming your board is of fixed size for simplicity.
    public int boardSizeX = 3;  // Adjust as per your needs
    public int boardSizeY = 3;
    public int boardSizeZ = 3;

    public GameObject boardPositionPrefab; // Your 3D grid unit

    private BoardPosition[,,] boardPositions;

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {
        boardPositions = new BoardPosition[boardSizeX, boardSizeY, boardSizeZ];

        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                for (int z = 0; z < boardSizeZ; z++)
                {
                    GameObject positionObj = Instantiate(boardPositionPrefab, new Vector3(x, y, z), Quaternion.identity);
                    boardPositions[x, y, z] = positionObj.GetComponent<BoardPosition>();
                }
            }
        }
    }

    void Update()
    {
        // Simple input logic to place a stone on click, you might want a more advanced system
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                BoardPosition hitPosition = hit.collider.GetComponent<BoardPosition>();
                if (hitPosition)
                {
                    PlaceStone(hitPosition);
                }
            }
        }
    }

    void PlaceStone(BoardPosition position)
    {
        if (position.currentState == BoardPosition.StoneState.None)
        {
            if (currentTurn == PlayerTurn.Black)
            {
                if (position.PlaceStone(BoardPosition.StoneState.Black))
                {
                    currentTurn = PlayerTurn.White;
                }
            }
            else
            {
                if (position.PlaceStone(BoardPosition.StoneState.White))
                {
                    currentTurn = PlayerTurn.Black;
                }
            }
        }
    }
}
