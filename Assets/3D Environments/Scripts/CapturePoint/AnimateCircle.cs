using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCircle : MonoBehaviour {

    public TriggerPoint triggerArea;
    public CapturePoint cap;
    [System.Serializable]
    public class RotPoint
    {
        public GameObject obj;
        public Vector3 ActiveRotation;
        public Vector3 NonActiveRotation;
    }
    public List<RotPoint> toRotate = new List<RotPoint>();

    public GameObject MainShow;
    public GameObject Piece;
    private RectTransform Init;
    private List<GameObject> ring = new List<GameObject>();
    public float res = 360;
    public bool autoRes;

    void Start()
    {
        Init = Piece.GetComponent<RectTransform>();
        if (autoRes)
        {
            res = cap.captureReq;
        }
        DecRing();
    }

	void Update () {
		foreach(RotPoint obj in toRotate)
        {
            if (triggerArea.active)
            {
                obj.obj.transform.Rotate(obj.ActiveRotation* Time.deltaTime*30);
            }
            else
            {
                obj.obj.transform.Rotate(obj.NonActiveRotation* Time.deltaTime*30);
            }
        }
        int ran = cap.captureReq;
        int setTo = cap.progress;
        Color colTo = TeamControl.getTeamById(cap.currentActive).Colour.color;
        if (cap.state != 0)
        {
            ran = cap.captureCap - cap.captureReq;
            setTo = Mathf.RoundToInt(cap.progressAmounts[cap.state].progress)-cap.captureReq;
            colTo = TeamControl.getTeamById(cap.state).Colour.color;
        }
        if (!autoRes)
        {
            float rat = res / ran;
            setTo = Mathf.RoundToInt(setTo*rat);
        }
        UpdateRing(setTo,colTo);
	}

    void DecRing()
    {
        for (int i = 0; i < res; i += 1)
        {
            var T = Instantiate(Piece);
            T.transform.parent = MainShow.transform;
            T.transform.localPosition = Piece.transform.localPosition;
            T.transform.localRotation = Piece.transform.localRotation;
            T.transform.localScale = Piece.transform.localScale;
            T.transform.Rotate(new Vector3(0,0,-i*(360/res)));
            ring.Add(T);
        }
    }
    void UpdateRing(int to,Color col)
    {
        for(int i = 0; i < res; i++)
        {
            if (i < ring.Count)
            {
                ring[i].transform.Find("Panel").GetComponent<Image>().color = col;
                if (i < to)
                {
                    ring[i].SetActive(true);
                }
                else
                {
                    ring[i].SetActive(false);
                }
            }
        }
    }
    
}
