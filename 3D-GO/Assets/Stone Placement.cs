using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePlacement : MonoBehaviour
{
    public GameObject blackStonePrefab;
    public GameObject whiteStonePrefab;
    private bool isBlackTurn = true;  // Starts with black's turn.

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is left mouse button.
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                PlaceStone(hit.point);
            }
        }
    }

    void PlaceStone(Vector3 position)
    {
        GameObject stoneToPlace = isBlackTurn ? blackStonePrefab : whiteStonePrefab;

        // Here, you might want to round the position to make the stone snap to your grid.
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position.z = Mathf.Round(position.z);

        Instantiate(stoneToPlace, position, Quaternion.identity);
        isBlackTurn = !isBlackTurn;  // Swap turns.
    }
}
