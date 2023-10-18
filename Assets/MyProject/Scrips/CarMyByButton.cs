using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMyByButton : MonoBehaviour
{
    public bool isPressed;          
    public float dampenPress = 0;   
    public float sensitivity = 2f;  
    void Start()
    {
        SetUpButton();              
    }

    void Update()
    {
        dampenPress = isPressed ? 1 : 0;

        //if (isPressed && dampenPress <=1f)
        //{
        //    dampenPress += sensitivity * Time.deltaTime;
        //}
        //else
        //{
        //    dampenPress = 0;
        //}
    }
    void SetUpButton()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => onClickDown());

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => onClickUp());
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    public void onClickDown()
    {
        isPressed = true;
    }

    public void onClickUp()
    {
        isPressed = false;
    }
}
