using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    public CheckPoint[] checkPoints;
    public int lap;
    public int checkPointId;
    public float dist;
    private void Update()
    {
        ReturnsDistance();
    }
    private void ReturnsDistance()
    {
        dist = Vector3.Distance(transform.position, checkPoints[checkPointId].transform.position);
    }
}
