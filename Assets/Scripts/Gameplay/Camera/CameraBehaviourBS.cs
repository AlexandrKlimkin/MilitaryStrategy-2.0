using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourBS : MonoBehaviour
{
    public float DefaultHeight;
    public float HeightDamping;
    public float DefaultAngle;
    public float HorizontalSensitivity;
    public float VerticalSensitivity;

    private float _TargetHeight;
    private Vector2 _HorPos;
    private float _Height;

    void Start()
    {
        transform.rotation = Quaternion.Euler(DefaultAngle, 90, 0);
        _HorPos = transform.position.ToVector2XZ();
        _Height = transform.position.y;
        _TargetHeight = _Height;
    }

    void Update()
    {
        var touchDelta = GameInput.Instance.ScreenInput.CurrentTouchDelta;
        var scrollDelta = GameInput.Instance.ScreenInput.ScrollDelta;
        HorizontalMove(touchDelta);
        VerticalMove(scrollDelta);

        _HorPos.x = Mathf.Clamp(_HorPos.x, -55, 5);
        _HorPos.y = Mathf.Clamp(_HorPos.y, -25f, 25f);

        _Height = Mathf.Lerp(_Height, _TargetHeight, Time.deltaTime * HeightDamping);
        transform.position = new Vector3(_HorPos.x, _Height, _HorPos.y);
    }

    private void HorizontalMove(Vector2 delta) {
        var deltaVec = delta * HorizontalSensitivity;
        _HorPos += new Vector2(deltaVec.y, deltaVec.x);
    }

    private void VerticalMove(Vector2 delta) {
        var sens = VerticalSensitivity * DefaultHeight / _Height;
        _TargetHeight -= delta.y * VerticalSensitivity;
    }
}