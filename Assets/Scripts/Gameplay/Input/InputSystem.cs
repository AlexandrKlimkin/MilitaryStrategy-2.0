using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : SingletonBehaviour<InputSystem> {

    public const float CameraRayLength = 100f;

    public event Action<Vector3> TouchGroundHit;
    public event Action<Actor> TouchActorHit;
    public event Action UseQAbility;
    public event Action RotateCameraRight;
    public event Action RotateCameraLeft;
    public event Action MoveCameraDown;
    public event Action MoveCameraUp;

    public event Action<float> MouseScroll;

    public RaycastHit CameraRaycastHit {
        get {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(cameraRay, out hit, CameraRayLength, Constants.Layers.Masks.Actor + Constants.Layers.Masks.Ground);
            return hit;
        }
    }

    protected override void Awake() {
        base.Awake();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var hit = CameraRaycastHit;
            if (hit.collider != null) {
                var hitObj = hit.collider.gameObject;

                if (Constants.Layers.Masks.Ground == (Constants.Layers.Masks.Ground | (1 << hitObj.layer))) {
                    TouchGroundHit?.Invoke(hit.point);
                }
                else if (Constants.Layers.Masks.Actor == (Constants.Layers.Masks.Actor | (1 << hitObj.layer))) {
                    var actor = hit.collider.gameObject.GetComponentInParent<Actor>();
                    TouchActorHit?.Invoke(actor);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            UseQAbility?.Invoke();
        }
        if (Input.GetKey(KeyCode.Q)) {
            RotateCameraLeft?.Invoke();
        }
        if (Input.GetKey(KeyCode.E)) {
            RotateCameraRight?.Invoke();
        }
        if (Input.GetKey(KeyCode.Z)) {
            MoveCameraDown?.Invoke();
        }
        if (Input.GetKey(KeyCode.X)) {
            MoveCameraUp?.Invoke();
        }
        if(Input.mouseScrollDelta != null) {
            if (Input.mouseScrollDelta.y != 0)
                MouseScroll?.Invoke(Input.mouseScrollDelta.y);
        }
    }
}