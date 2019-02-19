using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class movementControls : NetworkBehaviour{

    Rigidbody rig;
    NetworkChatPlayer chat;
    public Animator anim;
    public CameraOrbitControls cam;
    public CapsuleCollider col;
    public bool canMove;
    public float speed = 6.0f;
    public float walkSpeed = 3.0f;
    public float sprintSpeed = 10;
    [Range(0.1f,30f)]
    public float rotationSpeed = 2.0f;
    public bool isAiming = false;
    float rotateBy = 0.0f;
    [HideInInspector]
    public bool sprinting = false;
    [HideInInspector]
    public bool walking = false;
    public bool forceWalk = false;
    public bool canJump;
    bool grounded = true;
    public float jumpStrength = 5;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        chat = gameObject.GetComponent<NetworkChatPlayer>();
        if(cam == null)
        {
            cam = GetComponent<CameraOrbitControls>();
        }
    }
	
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        bool moveSet = canMove;
        bool jumpSet = canJump;
        if (canMove)
        {
            if (chat.inputField != null)
            {
                if (chat.inputField.isFocused)
                {
                    canMove = false;
                }
            }
        }
        if (canJump)
        {
            if (chat.inputField != null)
            {
                if (chat.inputField.isFocused)
                {
                    canJump = false;
                }
            }
        }
        isAiming = Input.GetButton("Aim");
        anim.SetBool("isAiming", isAiming);
        float axisZ = Input.GetAxis("Vertical");
        float axisX = Input.GetAxis("Horizontal");
        float translationZ = axisZ * speed;
        float translationX = axisX * speed;
        walking = false;
        sprinting = false;
        if (Input.GetButton("Sprint") && axisZ>0.5)
        {
            translationX = axisX * sprintSpeed;
            translationZ = axisZ * sprintSpeed;
            sprinting = true;
        }
        if ((Input.GetButton("Walk") && !sprinting)||isAiming||forceWalk)
        {
            translationX = axisX * walkSpeed;
            translationZ = axisZ * walkSpeed;
            walking = true;
        }
        translationX *= Time.deltaTime;
        translationZ *= Time.deltaTime;
        //transform.Rotate(0, rotation, 0);
        anim.SetFloat("axisZ", axisZ);
        anim.SetFloat("axisX", axisX);
        anim.SetBool("isWalking", walking);
        anim.SetBool("isSprinting", sprinting);
        if ((translationX != 0f || translationZ != 0f) && canMove)
        {
            RotateTo();
            float l = 0.4f;
            if (axisX > l || axisX < -l || axisZ > l || axisZ < -l)
            {
                transform.Translate(translationX, 0, translationZ);
            }
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (!grounded && rig.velocity.y == 0)
        {
            grounded = true;
        }
        anim.SetBool("isGrounded", grounded);
        if (Input.GetButtonDown("Jump") && grounded && canJump)
        {
            anim.SetBool("isJumping", true);
            if (anim.GetBool("isRunning"))
            {
                rig.AddForce(transform.up * jumpStrength);
            }
            grounded = false;
        }
        if (isAiming)
        {
            RotateTo();
        }
        canMove = moveSet;
        canJump = jumpSet;
    }
    public void RotateTo()
    {
        Quaternion rot = cam.rotPoint.rotation;
        rot.x = transform.rotation.x;
        rot.z = transform.rotation.z;
        Transform g = cam.rotPoint.parent;
        cam.rotPoint.parent = null;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot,rotationSpeed);
        cam.rotPoint.parent = g;
    }
}
