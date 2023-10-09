using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    public static CarUI Instance;

    public CarMyByButton gasPedal;
    public CarMyByButton brakePedal;
    public CarMyByButton leftButton;
    public CarMyByButton rightButton;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
