using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Width of each tile and offset to center of tile
    private const float tileWidth = 1.0f;
    private const float tileOffset = 0.5f;

    // List containing all the chess pieces
    public List<GameObject> pieces;

    // Currently selected tile. No selection provides -1.
    private int selectionX = -1;
    private int selectionY = -1;

    //private void Awake()
    //{
    //    GameObject boardObject = new GameObject("board");
    //    boardObject.transform.parent = transform;
    //    board = boardObject.AddComponent<Board>();
    //}

    // Use this for initialization
    void Start () {
		
	}
	
	/// Update is called once per frame
	void Update ()
    {
        updateSelection();
        drawChessBoard();
	}

    /// <summary>
    /// Updates the currently selected tile
    /// </summary>
    void updateSelection()
    {
        if (!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            selectionX = (int)(hit.point.x);
            selectionY = (int)(hit.point.y);
            Debug.Log(selectionX + ", " + selectionY);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    /// <summary>
    /// Draws the chessboard
    /// </summary>
    private void drawChessBoard ()
    {
        for (int i = 0; i <= 8; i++)
        {
            Vector2 lineWidth = Vector2.right * 8;
            Vector2 lineHeight = Vector2.up * 8;
            Vector2 startH = Vector2.up * i;
            Vector2 startV = Vector2.right * i;
            Debug.DrawLine(startH, startH + lineWidth, Color.blue);
            Debug.DrawLine(startV, startV + lineHeight, Color.blue);
        }

        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * selectionY, Vector2.right * (selectionX + 1) + Vector2.up * (selectionY + 1), Color.red);
            Debug.DrawLine(Vector2.right * selectionX + Vector2.up * (selectionY + 1), Vector2.right * (selectionX + 1) + Vector2.up * selectionY, Color.red);
        }
    }
}
