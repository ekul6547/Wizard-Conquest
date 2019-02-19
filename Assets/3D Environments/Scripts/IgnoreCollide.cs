using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollide : MonoBehaviour {

    public List<GameObject> toIgnore = new List<GameObject>();

	void OnCollisionEnter(Collision other)
    {
        if (toIgnore.Contains(other.gameObject))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.collider);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (toIgnore.Contains(other.gameObject))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.GetComponent<Collider>());
        }
    }

}
