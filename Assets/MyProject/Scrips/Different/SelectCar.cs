using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCar : MonoBehaviour
{
    public Transform[] body; 
    void Start()
    {
        print(PlayerPrefs.GetInt("SelectedCarID"));
        for(int i = 0; i < body.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("SelectedCarID"))
            {
                body[i].gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
