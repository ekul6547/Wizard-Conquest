using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraOrbitControls : NetworkBehaviour {

    GameObject player;
    public Transform link;
    public Transform rotPoint;
    public Transform ragPoint;
    public Camera cam;
    public Vector3 offset;
    public Vector3 lookOffset;
    public Vector3 AimOffset;
    [Range(1,3)]
    public float sprintPower;
    private float sprintPowerA = 1;
    [Range(0,10)]
    public float sensitivityX = 1;
    [Range(0, 10)]
    public float sensitivityY = 1;

    void Start () {
        player = gameObject;
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
	
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        float posChangeX = Input.GetAxis("Mouse X")*sensitivityX;
        float posChangeY = Input.GetAxis("Mouse Y")*-sensitivityY;
        var rot = rotPoint.localEulerAngles;
        if (rot.x + posChangeY < 10 || rot.x + posChangeY > 280)
        {
            rot.x += posChangeY;
        }
        rot.y += posChangeX;
        rot.z = 0;
        rotPoint.localEulerAngles = rot;
        Vector3 finalOffset = offset;
        if (!player.GetComponent<movementControls>().isAiming)
        {
            SprintPower(player.GetComponent<movementControls>().sprinting);
            finalOffset.z *= sprintPowerA;
        }
        link.localPosition = finalOffset;
        link.rotation = Quaternion.LookRotation((rotPoint.position) - link.position);
    }
    void SprintPower(bool i)
    {
        if (i)
        {
            if(sprintPowerA < sprintPower)
            {
                sprintPowerA += ((sprintPower - 1) / 3)*Time.deltaTime*30;
            }
        }
        else
        {
            if(sprintPowerA > 1)
            {
                sprintPowerA -= ((sprintPower - 1) / 3)*Time.deltaTime*30;
            }
        }
    }
    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetButton("Aim"))
        {
            rotPoint.localPosition = AimOffset;
        }
        else
        {
            rotPoint.localPosition = lookOffset;
        }
        if (!player.GetComponent<movementControls>().canMove)
        {
            rotPoint.position = ragPoint.position;
        }
        cam.transform.position = link.position;
        cam.transform.rotation = link.rotation;
    }
}
