using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SquadWidget : MonoBehaviour
{
    public Image PreviewImage;
    public GameObject SelectedBackground;
    public Text SquadSizeText;
    public Text CostText;

    private Button _Button;
    public SquadData SquadData { get; private set; }

    private void Awake() {
        _Button = GetComponent<Button>();
    }

    public void Initialize(SquadData squadData, UnityAction onClick) {
        SquadData = squadData;
        SquadSizeText.text = squadData.Size.x + "x" + squadData.Size.y;
        _Button.onClick.AddListener(onClick);
    }
    
    public void SetSelected(bool selected) {
        SelectedBackground.SetActive(selected);
    }

    private void OnDestroy() {
        _Button.onClick.RemoveAllListeners();
    }
}
