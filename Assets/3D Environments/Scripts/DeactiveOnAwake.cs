using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveOnAwake : MonoBehaviour {

    public Material invis;

    void Awake ()
    {
        if (invis != null)
        {
            gameObject.GetComponent<Renderer>().material = invis;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

	void Start () {
        if (invis != null)
        {
            gameObject.GetComponent<Renderer>().material = invis;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
