using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckPoint : MonoBehaviour
{
    //public float[] DistanceArrays;

    //[Header("Cars Car01 = Player Car")]
    //public Transform Car01;
    //public Transform Car02;
    //public Transform Car03;
    //public Transform Car04;

    //float First;
    //float Fourth;
    //float Second;
    //float Third;
    //public TextMeshProUGUI Postext;

    //public GameObject NextCheckPoint;
    void Start()
    {
        //NextCheckPoint.SetActive(false);
    }

    private void Update()
    {
        //DistanceArrays[0] = Vector3.Distance(transform.position, Car01.position);
        //DistanceArrays[1] = Vector3.Distance(transform.position, Car02.position);
        //DistanceArrays[2] = Vector3.Distance(transform.position, Car03.position);
        //DistanceArrays[3] = Vector3.Distance(transform.position, Car04.position);

        //SelectionSort(DistanceArrays);

        //First = DistanceArrays[0];
        //Second = DistanceArrays[1];
        //Third = DistanceArrays[2];
        //Fourth = DistanceArrays[3];

        //float Car01Dist = Vector3.Distance(transform.position, Car01.position);
        ////float Car02Dist = Vector3.Distance(transform.position, Car02.position);
        ////float Car03Dist = Vector3.Distance(transform.position, Car03.position);
        ////float Car04Dist = Vector3.Distance(transform.position, Car04.position);

        //if (Car01Dist == First)
        //{
        //    Postext.text = "1/4";
        //}
        //if (Car01Dist == Second)
        //{
        //    Postext.text = "2/4";
        //}
        //if (Car01Dist == Third)
        //{
        //    Postext.text = "3/4";
        //}
        //if (Car01Dist == Fourth)
        //{
        //    Postext.text = "4/4";
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        //Is it the Player who enters the collider?
        //if (other.tag == "Player")
        //{
        //    NextCheckPoint.SetActive(true);
        //    gameObject.SetActive(false);
        //}
        if (other.tag != "Player")
        {
            return;
        }
        if (transform == Laps.checkpointA[Laps.currentCheckpoint].transform)
        {
            //Check so we dont exceed our checkpoint quantity
            if (Laps.currentCheckpoint + 1 < Laps.checkpointA.Length)
            {
                //Add to currentLap if currentCheckpoint is 0
                if (Laps.currentCheckpoint == 0)
                {
                    Laps.currentLap++;
                }                 
                Laps.currentCheckpoint++;
            }
            else
            {
                //If we dont have any Checkpoints left, go back to 0
                Laps.currentCheckpoint = 0;
            }
        }


    }
    public void SelectionSort(float[] nums)
    {
        for (int i = 0; i < nums.Length - 1; i++)
        {
            int smallestNumIndex = i;
            for (int j = i + 1; j < nums.Length; j++)
            {
                if (nums[j] < nums[smallestNumIndex])
                    smallestNumIndex = j;
            }
            if (smallestNumIndex != i)
            {
                float temp = nums[i];
                nums[i] = nums[smallestNumIndex];
                nums[smallestNumIndex] = temp;
            }
        }
    }
}
