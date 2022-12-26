using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Laps : MonoBehaviour
{
    // These Static Variables are accessed in "checkpoint" Script
    public Transform[] checkPointArray;
    public static Transform[] checkpointA;
    public Record Player;
    //public TMP_Text TextLap;
    public GameObject winPanel;
    public static int currentCheckpoint = 0;
    public static int currentLap = 0;
    //public Vector3 startPos;
    public int Lap;
    public int checkPoint;

    void Start()
    {
        //startPos = transform.position;
        currentCheckpoint = 0;
        currentLap = 0;

    }

    void Update()
    {
        Lap = currentLap;
        if (0 < currentCheckpoint && currentCheckpoint < Player.checkPoints.Length)
            checkPoint = currentCheckpoint - 1;
        else
            checkPoint = 0;
        checkpointA = checkPointArray;
        //TextLap.text = "Checkpoint " + checkPoint.ToString() + " Lap " + Lap.ToString();
        Player.lap = Lap;
        Player.checkPointId = checkPoint;
        if (Lap == 2)
            winPanel.SetActive(true);
    }
}
