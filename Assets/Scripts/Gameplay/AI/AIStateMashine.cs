using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMashine : MonoBehaviour {

    public State CurrentState {
        get {
            return _CurrentState;
        }
        set {
            if (_CurrentState == null)
                _CurrentState = value;
            else {
                if(_CurrentState.GetType() != value.GetType()) {
                    _CurrentState.OnDeactivate();
                    _CurrentState = value;
                    _CurrentState.OnActivate();
                }
            }
        }
    }

    private void Update() {

    }

    private State _CurrentState;

	void Start () {
		
	}

}

public enum AIState { Idling, Moving, Purcueing, Fighting, TakingDamage }
