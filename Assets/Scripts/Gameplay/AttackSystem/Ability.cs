using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    public Unit Owner { get; protected set; }

    protected virtual void Awake()
    {
        Owner = GetComponentInParent<Unit>();
    }

    public abstract void UseAbility(); 
}
