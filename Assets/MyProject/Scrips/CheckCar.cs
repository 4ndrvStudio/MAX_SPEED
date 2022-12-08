using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCar : MonoBehaviour
{
    public int check;
    public WinGame checkWin;

    // Start is called before the first frame update
    void Start()
    {
        check = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player" && checkWin.check == check)
        {
            check++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
