using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetTextureScale : MonoBehaviour {
    public Renderer rend;
    public Vector2 scale = new Vector2(0.5F, 0.5F);
    public Vector2 offset; public Vector2 ScaleRef = new Vector2(5, 5);
    public bool attemptXYAutoScale = false; public bool attemptXZAutoScale = false;
    public bool attemptYZAutoScale = false;
    // Use this for initialization               
    void Start () {
        rend = GetComponent<Renderer> ();
    }
    // Update is called once per frame               
    void Update () {
        if (attemptXYAutoScale) {
            scale.x = transform.lossyScale.x / ScaleRef.x;
            scale.y = transform.lossyScale.y / ScaleRef.y;
        }
        if (attemptXZAutoScale) {
            scale.x = transform.lossyScale.x / ScaleRef.x;
            scale.y = transform.lossyScale.z / ScaleRef.y;
        }
        if (attemptYZAutoScale) { 
            scale.x = transform.lossyScale.y / ScaleRef.x;
            scale.y = transform.lossyScale.z / ScaleRef.y;
        }
        rend.material.mainTextureScale = scale;
        rend.material.mainTextureOffset = new Vector2(offset.x, offset.y);
    }
}
