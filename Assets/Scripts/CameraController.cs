using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MainCamera.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);

    }
}
