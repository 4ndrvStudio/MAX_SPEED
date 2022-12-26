using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManage : MonoBehaviour
{
    public List<CarAI> listCar;
    public Transform panelStart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!panelStart.gameObject.activeSelf)
        {
            foreach(CarAI c in listCar)
            {
                c.run = true;
            }
        }
    }
}
