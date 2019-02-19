using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProperties : MonoBehaviour {
    public damageSources DamageType;
    public float DamageAmount;
    public doesDamage doDamage;
    public bool constantDamage = false;
    public DamageAttributes[] Attributes = null;
}
public enum damageSources
{
    NORM = 0,
    FIRE = 1,
    ELECTRIC = 2,
    ABYSS = 3,
    HOLY = 4
}
public enum doesDamage
{
    DAMAGE = 0,
    HEAL = 1
}
public enum DamageAttributes
{
    BYPASSRESISTANCE = 0,
    DONTDISPLAYMESSAGE = 1
}

