using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_ChildCollision : MonoBehaviour {

    public bool triggerTrue = false;
    public bool collideTrue = false;
    public List<GameObject> collisionsWith = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            triggerTrue = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            triggerTrue = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            triggerTrue = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            collideTrue = true;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            collideTrue = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (collisionsWith.Contains(other.gameObject) || other.gameObject.tag == "Player")
        {
            collideTrue = false;
        }
    }
}
