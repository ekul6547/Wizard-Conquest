using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotDoorTrigger : MonoBehaviour {

    public List<GameObject> collisions = new List<GameObject>();
    public GameObject door;
    public bool open;
    public float openVel = 100;
    public float openForce = 10;
    public float closeVel = 100;
    public float closeForce = 10;
    


    void Update()
    {
        var childCollides = gameObject.GetComponentsInChildren<script_ChildCollision>();
        bool childCollidesBool = false;
        foreach(script_ChildCollision x in childCollides)
        {
            if(x.triggerTrue || x.collideTrue)
            {
                open = true;
                childCollidesBool = true;
            }
            if (!x.triggerTrue && !x.collideTrue && childCollidesBool == false)
            {
                open = false;
            }
        }

        JointMotor motor = door.GetComponent<HingeJoint>().motor;
        if (open == true)
        {
            motor.targetVelocity = openVel;
            motor.force = openForce;
        }
        else
        {
            motor.targetVelocity = -closeVel;
            motor.force = closeForce;
        }
        door.GetComponent<HingeJoint>().motor = motor;
        door.GetComponent<Rigidbody>().AddForce(Vector3.zero);
    }

    void OnTriggerStay(Collider other)
    {
        if (collisions.Contains(other.transform.root.gameObject) || other.transform.root.gameObject.tag == "Player")
        {
            open = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (collisions.Contains(other.transform.root.gameObject) || other.transform.root.gameObject.tag == "Player")
        {
            open = false;
        }
    }
}
