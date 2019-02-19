using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnProperties : NetworkBehaviour {

    public int team = 0;
    int cool = 200;

    void LateUpdate()
    {
        if (cool > 0) { cool -= 1; }
        else if (cool == 0)
        {
            cool -= 1;
            foreach(Collider col in gameObject.GetComponents<Collider>())
            {
                col.enabled = true;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player")
        {
            EntityProperties prop = other.transform.root.gameObject.GetComponent<EntityProperties>();
            if (prop != null)
            {
                if (prop.team == 0)
                {
                    prop.team = team;
                    prop.spawnPoint = this.transform.root;
                }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.transform.root.gameObject.tag == "Player")
        {
            EntityProperties prop = other.transform.root.gameObject.GetComponent<EntityProperties>();
            if (prop != null)
            {
                if (prop.team == 0)
                {
                    prop.team = team;
                    prop.spawnPoint = this.transform.root;
                }
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        EntityProperties prop = other.transform.root.gameObject.GetComponent<EntityProperties>();
        if (prop != null)
        {
            if (prop.team == team || prop.team == 0)
            {
                Physics.IgnoreCollision(gameObject.GetComponents<Collider>()[1], other.collider);
            }
        }
    }
}
