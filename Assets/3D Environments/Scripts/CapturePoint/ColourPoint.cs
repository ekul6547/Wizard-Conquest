using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class ColourPoint : NetworkBehaviour {

    public GameObject particleObject;
    public float activeRate;
    public float nonActiveRate;
    public TextMesh displayText;

    public List<GameObject> toColour = new List<GameObject>();
    public int state;
    public CapturePoint linkedPoint = null;
    public TriggerPoint trigger;

    private GameObject cam;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

	void Update () {
        if(linkedPoint != null && Application.isPlaying)
        {
            state = linkedPoint.state;
        }
        ColourIn();
	}

    void ColourIn()
    {
        if(state < 0)
        {
            state = 0;
        }
        TeamControl.teamStat teamTo;
        if (Application.isEditor)
        {
            teamTo = GameObject.FindGameObjectWithTag("GameController").GetComponent<TeamManager>().getTeamById(state);
        }
        else
        {
            teamTo = TeamControl.getTeamById(state);
        }
        Color col = new Color(0, 0, 0, 255);
        foreach(GameObject x in toColour)
        {
            var renders = x.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer y in renders)
            {
                if (teamTo != null)
                {
                    y.sharedMaterial = teamTo.Colour;
                    col = y.sharedMaterial.color;
                }
            }
        }
        ParticleSystem ps = particleObject.GetComponent<ParticleSystem>();
        var trails = ps.trails;
        trails.colorOverLifetime = col;
        var emit = ps.emission;
        if (trigger.active)
        {
            emit.rateOverTime = activeRate;
        }
        else
        {
            emit.rateOverTime = nonActiveRate;
        }
        displayText.text = linkedPoint.PointNick;
        displayText.color = col;
        displayText.transform.rotation = Quaternion.LookRotation(displayText.transform.position - cam.transform.position);
    }
}