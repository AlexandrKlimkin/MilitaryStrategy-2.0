using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInputWidget : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public Vector2 CurrentTouchDelta { get; private set; }
    public Vector2 ScrollDelta { get; private set; }

#if UNITY_EDITOR || UNITY_STANDALONE
    private const float MouseSpeed = 0.45f;
#endif
    private const float TouchSpeedNormal = 0.65f;

    private bool _HasTouch;
    private int _PointerID;
    private Vector2 _TargetTouchDelta;

#if UNITY_EDITOR || UNITY_STANDALONE
    private Vector2 _PreviousMousePos;
#endif

    private void Update() {
        if (_HasTouch) {
#if UNITY_EDITOR || UNITY_STANDALONE
            _TargetTouchDelta = (Input.mousePosition.ToVector2XY() - _PreviousMousePos) * MouseSpeed;
#else
            for (int i = 0; i < Input.touches.Length; i++) {
                if (Input.touches[i].fingerId == _PointerID) {
                    _TargetTouchDelta = Input.touches[i].deltaPosition;
                    break;
                }
            }
#endif
            _TargetTouchDelta *= TouchSpeedNormal;
            _TargetTouchDelta.y *= -1;
        }
        else {
            _TargetTouchDelta = Vector2.zero;
        }
        CurrentTouchDelta = Vector2.Lerp(CurrentTouchDelta, _TargetTouchDelta, Time.deltaTime * 15);
#if UNITY_EDITOR || UNITY_STANDALONE
        _PreviousMousePos = Input.mousePosition;
        ScrollDelta = Input.mouseScrollDelta;
#endif
    }

    private void OnDisable() {
        OnPointerUp(new PointerEventData(EventSystem.current) { pointerId = _PointerID });
        CurrentTouchDelta = Vector2.zero;
        _TargetTouchDelta = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (_HasTouch == false) {
            _HasTouch = true;
            _PointerID = eventData.pointerId;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (_HasTouch == true && _PointerID == eventData.pointerId) {
            _HasTouch = false;
            _TargetTouchDelta = Vector2.zero;
        }
    }
}