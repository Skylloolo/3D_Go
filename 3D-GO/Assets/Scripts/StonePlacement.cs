using UnityEngine;

public class BoardPosition : MonoBehaviour
{
    public int xIndex, yIndex, zIndex;
    public StoneState currentState = StoneState.None;

    public GameObject whiteStonePrefab;
    public GameObject blackStonePrefab;

    private GameObject currentStone;

    public bool PlaceStone(StoneState newState)
    {
        // If the position is already occupied, do nothing and return false (invalid move).
        if (currentState != StoneState.None)
        {
            return false;
        }

        // Place new stone.
        if (newState == StoneState.White)
        {
            currentStone = Instantiate(whiteStonePrefab, transform.position, Quaternion.identity, transform);
        }
        else if (newState == StoneState.Black)
        {
            currentStone = Instantiate(blackStonePrefab, transform.position, Quaternion.identity, transform);
        }

        currentState = newState;
        return true;  // Valid move.
    }

    public void RemoveStone()
    {
        if (currentStone != null)
        {
            Destroy(currentStone); // Removes the stone GameObject from the scene
            currentStone = null;
            currentState = StoneState.None;
        }
    }
}