using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoneState { None, Black, White }

public class GameController : MonoBehaviour
{
    public enum PlayerTurn { Black, White }
    public PlayerTurn currentTurn = PlayerTurn.Black;

    public int boardSizeX = 3;  // Adjust as per your needs
    public int boardSizeY = 3;
    public int boardSizeZ = 3;

    public GameObject boardPositionPrefab; // 3D grid unit

    private BoardPosition[,,] boardPositions;
    private StoneState[,,] boardStates;

    // Directions: up, down, left, right, forward, backward
    public Vector3Int[] directions = {Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back};

    void Start()
    {
        InitializeBoard();
        InitializeBoardStates();
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
                    BoardPosition boardPos = positionObj.GetComponent<BoardPosition>();
                    boardPos.xIndex = x;
                    boardPos.yIndex = y;
                    boardPos.zIndex = z;
                    boardPositions[x, y, z] = boardPos;
                }
            }
        }
    }

    void InitializeBoardStates()
    {
        boardStates = new StoneState[boardSizeX, boardSizeY, boardSizeZ];
        for (int x = 0; x < boardSizeX; x++)
        {
            for (int y = 0; y < boardSizeY; y++)
            {
                for (int z = 0; z < boardSizeZ; z++)
                {
                    boardStates[x, y, z] = StoneState.None;
                }
            }
        }
    }
    
    void Update()
    {
        // Simple input logic to place a stone on click for now
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
        if (position.currentState == StoneState.None)
        {
            StoneState newState = (currentTurn == PlayerTurn.Black) ? StoneState.Black : StoneState.White;

            if (position.PlaceStone(newState))
            {
                // Update the board state
                boardStates[position.xIndex, position.yIndex, position.zIndex] = newState;

                // Check for captures
                CheckForCaptures(position.xIndex, position.yIndex, position.zIndex);

                // Switch turns
                currentTurn = (currentTurn == PlayerTurn.Black) ? PlayerTurn.White : PlayerTurn.Black;
            }
        }
    }

    bool IsGroupSurrounded(int x, int y, int z)
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        return CheckSurrounding(x, y, z, visited);
    }

    bool CheckSurrounding(int x, int y, int z, HashSet<Vector3Int> visited)
    {
        Vector3Int currentPosition = new Vector3Int(x, y, z);
        if (visited.Contains(currentPosition)) return true; // Already checked this position
        visited.Add(currentPosition);

        StoneState stoneState = boardStates[x, y, z];
        
        foreach (var direction in directions)
        {
            Vector3Int adjacentPosition = currentPosition + direction;
            if (!IsValidPosition(adjacentPosition))
            {
                continue; // Skip this direction as it's off the board
            }

            StoneState adjacentStoneState = boardStates[adjacentPosition.x, adjacentPosition.y, adjacentPosition.z];
            if (adjacentStoneState == StoneState.None)
            {
                return false; // Found a liberty, group is not surrounded
            }
            else if (adjacentStoneState == stoneState && !visited.Contains(adjacentPosition))
            {
                // Continue checking the rest of the group
                if (!CheckSurrounding(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z, visited))
                {
                    return false; // Found a liberty in the connected group
                }
            }
        }

        return true; // No liberties found, group is surrounded
    }

    bool IsValidPosition(Vector3Int position)
    {
        return position.x >= 0 && position.x < boardSizeX &&
               position.y >= 0 && position.y < boardSizeY &&
               position.z >= 0 && position.z < boardSizeZ;
    }


    void CheckForCaptures(int x, int y, int z)
    {
        StoneState lastPlacedStone = boardStates[x, y, z];
        StoneState opponentColor = (lastPlacedStone == StoneState.Black) ? StoneState.White : StoneState.Black;

        foreach (var direction in directions)
        {
            Vector3Int adjacentPosition = new Vector3Int(x, y, z) + direction;
            if (IsValidPosition(adjacentPosition))
            {
                if (boardStates[adjacentPosition.x, adjacentPosition.y, adjacentPosition.z] == opponentColor)
                {
                    // Check if the opponent's stone at this position is part of a surrounded group
                    if (IsGroupSurrounded(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z))
                    {
                        // Capture the surrounded group
                        CaptureGroup(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z, opponentColor);
                    }
                }
            }
        }
    }

    void CaptureGroup(int x, int y, int z, StoneState color)
    {
        HashSet<Vector3Int> group = new HashSet<Vector3Int>();
        FindGroup(x, y, z, color, group);

        foreach (var position in group)
        {
            boardStates[position.x, position.y, position.z] = StoneState.None;
            // Remove the stone visually from the board
            BoardPosition boardPos = boardPositions[position.x, position.y, position.z];
            boardPos.RemoveStone(); 
        }
    }

    void FindGroup(int x, int y, int z, StoneState color, HashSet<Vector3Int> group)
    {
        Vector3Int currentPosition = new Vector3Int(x, y, z);
        if (group.Contains(currentPosition)) return;
        group.Add(currentPosition);

        foreach (var direction in directions)
        {
            Vector3Int adjacentPosition = currentPosition + direction;
            if (IsValidPosition(adjacentPosition))
            {
                if (boardStates[adjacentPosition.x, adjacentPosition.y, adjacentPosition.z] == color)
                {
                    FindGroup(adjacentPosition.x, adjacentPosition.y, adjacentPosition.z, color, group);
                }
            }
        }
    }

}
