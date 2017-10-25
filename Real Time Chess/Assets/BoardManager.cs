using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public Board board;

    private void Awake()
    {
        GameObject boardObject = new GameObject("board");
        boardObject.transform.parent = transform;
        board = boardObject.AddComponent<Board>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
