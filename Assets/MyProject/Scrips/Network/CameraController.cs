using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FishNet.Object;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

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

    public void SetFollow(Transform trans)
    {
        _virtualCamera.Follow = trans;
    }
}
