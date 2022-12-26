using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownTimer;
    public float time;
    void Start()
    {
        time = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 1)
        {
            countdownTimer.text = "GO";
        }
        else
            countdownTimer.text = ((int)time).ToString();
        if (time < 0)
            transform.gameObject.SetActive(false);
        time -= Time.deltaTime;
        
    }
}
