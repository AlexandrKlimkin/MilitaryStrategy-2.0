using System;
using UnityEngine;

public abstract class State {
    public abstract AIState Type { get; set; }
    public abstract void OnActivate();
    public abstract void OnExecution();
    public abstract void OnDeactivate();
}