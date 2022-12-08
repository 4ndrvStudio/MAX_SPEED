using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public int NumberOfRound;
    public int check;
    public CheckCar checkCar;
    public GameObject WinPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && check < checkCar.check)
        {
            check++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        check = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (check == NumberOfRound)
        {
            WinPanel.SetActive(true);
        }
    }
}
