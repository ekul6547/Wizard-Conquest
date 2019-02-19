using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class attackControls : NetworkBehaviour {

    public Animator anim;
    movementControls Move;

    void Awake()
    {
        Move = GetComponent<movementControls>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        int attackID = 0;
        if (Input.GetButton("FireSpell1"))
        {
            attackID = 1;
        }
        anim.SetInteger("AttackID", attackID);
        anim.SetBool("isAttacking", attackID != 0);
    }
}
