using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour {

    public int team;
    public bool active;

    List<Collider> TriggerList = new List<Collider>();

    void Update()
    {
        if (TriggerList.Count > 0)
        {
            EntityProperties first = TriggerList[0].transform.root.GetComponent<EntityProperties>();
            bool shouldActive = first.canTriggerPoint;
            int shouldTeam = first.team;
            foreach (Collider col in TriggerList)
            {
                EntityProperties prop = col.transform.root.GetComponent<EntityProperties>();
                if (prop != null)
                {
                    if (prop.canTriggerPoint)
                    {
                        if(prop.team != first.team)
                        {
                            shouldActive = false;
                            shouldTeam = 0;
                        }
                    }
                }
            }
            team = shouldTeam;
            active = shouldActive;
        }
        else
        {
            team = 0;
            active = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!TriggerList.Contains(other) && other.transform.root.gameObject.tag == "Player")
        {
            if (other.transform.root.GetComponent<EntityProperties>().canTriggerPoint)
            {
                TriggerList.Add(other);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (TriggerList.Contains(other))
        {
            TriggerList.Remove(other);
        }
    }

}
