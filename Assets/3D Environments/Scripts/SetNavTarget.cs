using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetNavTarget : MonoBehaviour {

    public GameObject target;
    
	void Update () {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
	}
}
