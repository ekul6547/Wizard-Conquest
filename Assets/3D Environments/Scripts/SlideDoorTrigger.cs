using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorTrigger : MonoBehaviour {

    public GameObject door;
    private Vector3 start;
    public Vector3 scaleAmount;
    private Vector3 end;
    public bool open;
    public bool invert = false;
    [Range(0,1)]
    public float raiseSpeedPercent = 0.25f;

    [Range(0, 1)]
    public float fallSpeedPercent = 0.25f;

    void Start ()
    {
        start = door.transform.localScale;
        end = start - scaleAmount;
    }

    void Update()
    {
        Vector3 scales = door.transform.localScale;
        if ((open && invert == false) || (!open && invert == true))
        {
            if(scales != end)
            {
                if (scales.x > end.x){ scales.x -= scaleAmount.x * raiseSpeedPercent * Time.deltaTime * 30; }
                if (scales.y > end.y){ scales.y -= scaleAmount.y * raiseSpeedPercent * Time.deltaTime * 30; }
                if (scales.z > end.z){ scales.z -= scaleAmount.z * raiseSpeedPercent * Time.deltaTime * 30; }
            }
        }
        else
        {
            if (scales != start)
            {
                if (scales.x < start.x){ scales.x += scaleAmount.x * fallSpeedPercent * Time.deltaTime * 30; }
                if (scales.y < start.y){ scales.y += scaleAmount.y * fallSpeedPercent * Time.deltaTime * 30; }
                if (scales.z < start.z){ scales.z += scaleAmount.z * fallSpeedPercent * Time.deltaTime * 30; }
            }
        }
        door.transform.localScale = scales;
    }

	void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            open = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player")
        {
            open = false;
        }
    }
}
