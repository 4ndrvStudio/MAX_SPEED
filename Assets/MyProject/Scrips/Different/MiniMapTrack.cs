using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapTrack : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject TrackPath;

    public GameObject Player;
    public GameObject MiniMapCam;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        TrackPath = this.gameObject;

        int num_of_path = TrackPath.transform.childCount;
        lineRenderer.positionCount = num_of_path + 1;
        for (int i = 0; i < num_of_path; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(TrackPath.transform.GetChild(i).transform.position.x, 4, TrackPath.transform.GetChild(i).transform.position.z));
        }
        lineRenderer.SetPosition(num_of_path, lineRenderer.GetPosition(0));
        lineRenderer.startWidth = 15f;
        lineRenderer.endWidth = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        MiniMapCam.transform.position = (new Vector3(Player.transform.position.x+20, MiniMapCam.transform.position.y, Player.transform.position.z));
    }
}
