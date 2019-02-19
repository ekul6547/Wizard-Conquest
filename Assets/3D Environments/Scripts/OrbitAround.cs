using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour {

    public Transform Centre;
    public float radius;
    public float speed;

	void Update () {
        transform.position = new Vector3(((float)System.Math.Sin((Time.time+30) * speed) * radius), transform.position.y, ((float)System.Math.Sin((Time.time) * speed) * radius));
    }
}
