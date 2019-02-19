using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ColourPoint))]
public class CapturePoint : NetworkBehaviour
{
    ColourPoint cp;
    AudioSource Sound_Capture;

    public string PointName = "Capture Point";
    public string PointNick = "CP";
    [SyncVar]
    protected int _state;

    public int state
    {
        get { return _state; }
        set { setState(value); }
    }
    public TriggerPoint area;

    [System.Serializable]
    public struct CapProgress
    {
        public int team;
        public float progress;
        public CapProgress (int ID, float P)
        {
            team = ID;
            progress = P;
        }
        public void setProgress(float i)
        {
            progress = i;
        }
        public void addProgress(float i)
        {
            progress += i;
        }
        public void addFixedProgress()
        {
            progress += 1 * (Time.deltaTime * 30);
        }
        public void subFixedProgress()
        {
            progress -= 1 * (Time.deltaTime * 30);
            if (progress < 0)
                progress = 0;
        }
    }
    public class SyncListProg : SyncListStruct<CapProgress>
    {
    }
    [SyncVar]
    public SyncListProg progressAmounts = new SyncListProg();

    public int captureReq = 1000;
    public int captureCap = 1500;

    [SyncVar]
    public int currentActive;
    [SyncVar]
    public int progress;

    void setState(int s)
    {
        if(_state != s)
        {
            Sound_Capture.Play();
        }

        _state = s;
    }

    void Start()
    {
        for(int i = 0;i < 5; i++)
        {
            progressAmounts.Add(new CapProgress(i, 0));
        }
        Sound_Capture = GetComponent<AudioSource>();
    }

    [Command]
    void CmdAddAmount(int team, int amount)
    {
        CapProgress T = progressAmounts[team];
        T.progress += amount;
    }
    [Command]
    void CmdOverrideAmount(int team, CapProgress amount)
    {
        progressAmounts[team] = amount;
    }

    public CapProgress getTeamByID(int ID)
    {
        CapProgress t = new CapProgress(0,0);
        foreach(CapProgress s in progressAmounts)
        {
            if(s.team == ID)
            {
                t = s;
                return t;
            }
        }
        return t;
    }

    bool isTeamRegistered (int ID)
    {
        foreach(CapProgress c in progressAmounts)
        {
            if(c.team == ID)
            {
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        currentActive = 0;
        int isState = 0;
        if (area.active)
        {
            currentActive = area.team;
        }
        CapProgress S = getTeamByID(state);
        for (int i = 0; i < progressAmounts.Count; i++) {
            CapProgress I = progressAmounts[i];
            if (area.active)
            {
                if (area.team==I.team)
                {
                    I.addFixedProgress();
                }
                else
                {
                    I.subFixedProgress();
                }
            }
            if (I.progress > captureReq)
            {
                isState = i;
            }
            if(I.progress > captureCap)
            {
                I.setProgress(captureCap);
            }
            CmdOverrideAmount(i, I);
        }
        for (int i = 0; i < progressAmounts.Count; i++)
        {
            if (S.progress > captureReq)
            {
                if (progressAmounts[i].team != S.team)
                {
                    progressAmounts[i].setProgress(0);
                }
            }
        }
        setState(isState);
        progress = Mathf.RoundToInt(getTeamByID(area.team).progress);
    }
}
