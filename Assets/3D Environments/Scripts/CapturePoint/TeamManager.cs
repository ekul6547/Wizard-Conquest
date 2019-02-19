using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TeamControl {

    [System.Serializable]
    public class teamStat
    {
        public string Name = "Team Name";
        public Material Colour;
        public Material Character;
    }

    public static List<teamStat> teams = new List<teamStat>();

    public static teamStat getTeamByName(string name)
    {
        foreach(teamStat t in teams)
        {
            if(t.Name == name)
            {
                return t;
            }
        }
        if (teams.Count <= 0)
        {
            return null;
        }
        return teams[0];
    }

    public static teamStat getTeamById(int id)
    {
        if(id >= teams.Count || id < 0)
        {
            if (teams.Count <= 0)
            {
                return null;
            }
            return teams[0];
        }
        else
        {
            return teams[id];
        }
    }
    
}

[ExecuteInEditMode]
public class TeamManager : MonoBehaviour
{
    public List<TeamControl.teamStat> teams = new List<TeamControl.teamStat>(); //0 must be Nuetral

    void Awake()
    {
        TeamControl.teams.Clear();
        foreach (TeamControl.teamStat t in teams)
        {
            if (!TeamControl.teams.Contains(t))
            {
                TeamControl.teams.Add(t);
            }
        }
    }

    public TeamControl.teamStat getTeamByName(string name)
    {
        foreach (TeamControl.teamStat t in teams)
        {
            if (t.Name == name)
            {
                return t;
            }
        }

        if (teams.Count <= 0)
        {
            return null;
        }
        return teams[0];
    }

    public TeamControl.teamStat getTeamById(int id)
    {
        if (id >= teams.Count || id < 0)
        {
            if(teams.Count <= 0) {
                return null;
            }
            return teams[0];
        }
        else
        {
            return teams[id];
        }
    }
}
