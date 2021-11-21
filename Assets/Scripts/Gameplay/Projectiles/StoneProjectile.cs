using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProjectile : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody>();
    }
}
