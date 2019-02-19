using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EntityProperties : NetworkBehaviour
{
    public bool canTriggerPoint;
    [SyncVar]
    public int team;
    public Animator anim;
    movementControls Move;
    private List<Collision> CollideList = new List<Collision>();
    public List<GameObject> toColour = new List<GameObject>();
    public Rigidbody[] ragList = new Rigidbody[0];
    public bool enableHealthSystem;
    public PlayerHUD HUD;
    [Header("Damage System")]
    [SyncVar]
    public float health = 0f;
    public float MaxHealth = 100f;
    [SyncVar]
    public bool Immortal = false;
    public float resCounter = 0;
    [HideInInspector]
    public Transform spawnPoint;
    [System.Serializable]
    public class DamageResistance
    {
        public damageSources type;
        [Range(-100,100)]
        public float resistance = 0;
        public bool invertHealDamage = false;

        public DamageResistance(damageSources t, float r)
        {
            type = t;
            resistance = r;
        }
    }
    public List<DamageResistance> toResist = new List<DamageResistance>();
    public Dictionary<damageSources, DamageResistance> Resistances = new Dictionary<damageSources, DamageResistance>();
    [Header("Mana System")]
    [SyncVar]
    public float Mana;
    public float MaxMana = 100;
    public bool canDrainMana = true;
    
    void Awake()
    {
    }
    
    void Start()
    {
        RpcEnableRag();
        Move = GetComponent<movementControls>();
        if (enableHealthSystem)
        {
            InitRes();
            if (health == 0f)
            {
                CmdSetHealth(MaxHealth);
            }
        }
        RpcDisableRag();
        if (isLocalPlayer)
        {
            GameObject HUDO = GameObject.FindGameObjectWithTag("HUD");
            if (HUDO != null)
            {
                HUD = HUDO.GetComponent<PlayerHUD>();
                HUD.prop = gameObject.GetComponent<EntityProperties>();
            }
        }
    }

    void Update()
    {
        ColourChar();
        if(transform.position.y < -20 && resCounter == 0)
        {
            StartRespawn();
        }
        if (enableHealthSystem)
        {
            if(health <= 0f && resCounter == 0)
            {
                StartRespawn();
            }
        }
        if(resCounter > 0)
        {
            resCounter += 1 * Time.deltaTime;
        }
        if(resCounter >= 5)
        {
            resCounter = 0;
            RpcRespawn();
        }
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RpcEnableRag();
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                RpcDisableRag();
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        DamageProperties dProp = other.gameObject.GetComponent<DamageProperties>();
        if(dProp != null)
        {
            if (!dProp.constantDamage)
            {
                Attack(dProp);
            }
        }
    }
    void OnCollisionStay(Collision other)
    {
        DamageProperties dProp = other.gameObject.GetComponent<DamageProperties>();
        if (dProp != null)
        {
            if (dProp.constantDamage)
            {
                Attack(dProp);
            }
        }
    }
    void StartRespawn()
    {
        RpcEnableRag();
        resCounter = 0.0001f;
        Move.canMove = false;
        canTriggerPoint = false;
    }
    void RpcRespawn()
    {
        if (spawnPoint != null)
        {
            transform.position = new Vector3(spawnPoint.position.x + UnityEngine.Random.Range(-1, 1), spawnPoint.position.y + 0.25f, spawnPoint.position.z + UnityEngine.Random.Range(-1, 1));
        }
        else
        {
            transform.position = Vector3.zero;
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (enableHealthSystem)
        {
            CmdSetHealth(MaxHealth);
        }
        RpcDisableRag();
        Move.canMove = true;
        canTriggerPoint = true;
    }
    [ContextMenu("Disable Rag")]
    [ClientRpc]
    void RpcDisableRag()
    {
        foreach(Rigidbody j in ragList)
        {
            j.isKinematic = true;
            j.GetComponent<Collider>().enabled = false;
        }
        anim.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    [ContextMenu("Enable Rag")]
    [ClientRpc]
    void RpcEnableRag()
    {
        foreach (Rigidbody j in ragList)
        {
            j.isKinematic = false;
            j.GetComponent<Collider>().enabled = true;
        }
        anim.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void Attack(DamageProperties damage)
    {
        if (damage.doDamage == doesDamage.HEAL)
        {
            Heal(damage.DamageType, damage.DamageAmount);
        }else if(damage.doDamage == doesDamage.DAMAGE)
        {
            Damage(damage.DamageType, damage.DamageAmount,damage.Attributes);
        }
    }
    [Command]
    public void CmdSetHealth(float Amount)
    {
        health = Amount;
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
    }
    public void Damage(damageSources source, float amount,DamageAttributes[] atts)
    {
        if (!Immortal)
        {
            float setTo = amount;
            float resSet = 100-Resistances[source].resistance;
            if (atts != null)
            {
                foreach(DamageAttributes D in atts)
                {
                    if(D == DamageAttributes.BYPASSRESISTANCE)
                    {
                        resSet = 100;
                    }
                }
            }
            setTo = setTo * (resSet / 100);
            if (Resistances[source].invertHealDamage)
            {
                setTo *= -1;
            }
            CmdSetHealth(health-setTo);
        }
    }
    public void Damage(float amount)
    {
        Damage(damageSources.NORM, amount,null);
    }
    public void Heal(damageSources source, float amount)
    {
        float setTo = amount;
        setTo = setTo * (Resistances[source].resistance/100);
        if (Resistances[source].invertHealDamage)
        {
            setTo *= -1;
        }
        CmdSetHealth(health + setTo);
    }
    public void Heal(float amount)
    {
        Heal(damageSources.NORM, amount);
    }
    [ContextMenu("InitRes")]
    void InitRes()
    {
        Resistances.Clear();
        foreach(damageSources source in Enum.GetValues(typeof(damageSources)))
        {
            Resistances.Add(source, new DamageResistance(source,0));
        }
        foreach(DamageResistance source in toResist)
        {
            if (source.resistance >= 0)
            {
                Resistances[source.type] = source;
            }
            else
            {
                Resistances[source.type] = new DamageResistance(source.type,source.resistance);
            }
        }
    }
    void ColourChar()
    {
        TeamControl.teamStat col = TeamControl.getTeamById(team);
        foreach(GameObject o in toColour)
        {
            o.GetComponent<Renderer>().sharedMaterial = col.Character;
        }
    }
}