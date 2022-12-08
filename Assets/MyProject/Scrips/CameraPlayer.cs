using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform player;
    public Transform cameraTarget;
    public Vector3 Offset;
    public float speed;

    void FixedUpdate()
    {
        Vector3 dPos = cameraTarget.position + Offset;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos + player.transform.TransformVector(Offset), speed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(player.position);
    }
}
