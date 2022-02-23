using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    private Actor _Player;
    private Slider _Slider;
    public bool Initialized { get; private set; }

    private void Awake() {
        StartCoroutine(InitializeRoutine());
    }

    //ToDo: remove this shit, make signal subscription
    private IEnumerator InitializeRoutine() {
        _Slider = GetComponent<Slider>();
        yield return new WaitUntil(() => PlayerController.Instance != null);
        yield return new WaitUntil(() => PlayerController.Instance.Unit != null);
        _Player = PlayerController.Instance.Unit;
        if(_Player)
            _Player.OnHealthChanged += UpdateBar;
        Initialized = true;
    }

    private void UpdateBar() {
        _Slider.value = _Player.RelativeHealth;
    }

    private void OnDestroy() {
        if(_Player)
            _Player.OnHealthChanged -= UpdateBar;
    }
}
