using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customizer : MonoBehaviour
{
    public static Customizer Instance;

    [Header("White Pieces")]
    public Color whitePiecesOutlineColor;
    public Color32 whitePiecesFillColor;

    public Gradient whitePieceGradient;

    [Header("Black Pieces")]
    public Color blackPiecesOutlineColor;
    public Color32 blackPiecesFillColor;

    public Gradient blackPieceGradient;

    [Header("Game Board")]
    public Color32 checkerColor;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
