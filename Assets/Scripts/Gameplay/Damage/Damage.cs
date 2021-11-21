using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage {
    public Actor Instigator;
    public float Amount;
    public DamageType Type;

    public Damage(float amount, Actor instigator = null, DamageType type = DamageType.Small) {
        Amount = amount;
        Instigator = instigator;
        Type = type;
    }
}

public enum DamageType { Small, Middle, Big }