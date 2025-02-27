using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    bool cameraMoving = false;
    public void setCameraMoving(bool value)
    {
        cameraMoving = value;
    }
    GameObject player;
    // Update is called once per frame
    void Awake()
    {
        player = GameManager.Instance.player;
    }
    void Update()
    {
        if (cameraMoving)
        {
            Vector3 myPos = new Vector3(player.transform.position.x, 1.5f, -10);
            transform.position = myPos;
        }
    }
    
}
