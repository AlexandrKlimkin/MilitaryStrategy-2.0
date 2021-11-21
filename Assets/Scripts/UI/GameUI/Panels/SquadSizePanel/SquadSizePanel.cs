using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class SquadSizePanel : MonoBehaviour
{
    public SquadWidget _SquadWidgetTemplate;
    public Transform WidgetsContainer;
    private List<SquadWidget> _SquadWidgets = new List<SquadWidget>();
    //private List<SquadData> _AvailableSquads;
    public SquadData SelectedSquadData { get; private set; }

    private void Start() {
        var availableSquads = Resources.Load<AvailableSquadsConfig>("Squads/LightInfantry/AvailableSquadsConfig").AvaliableSquads;
        ClearPanel();
        CreateWidgets(availableSquads);
        if (_SquadWidgets.Count > 0)
            SelectWidget(_SquadWidgets[0]);
    }

    private void CreateWidgets(List<SquadData> squadsData) {
        foreach(var squad in squadsData) {
            var widget = Instantiate(_SquadWidgetTemplate, WidgetsContainer);
            widget.Initialize(squad, () => { SelectWidget(widget); });
            _SquadWidgets.Add(widget);
        }
    }

    private void ClearPanel() {
        for(int i = 0; i < WidgetsContainer.childCount; i++) {
            Destroy(WidgetsContainer.GetChild(i).gameObject); //ToDo: Pool system
        }
    }

    private void SelectWidget(SquadWidget squadWidget) {
        _SquadWidgets.ForEach(_ => _.SetSelected(_ == squadWidget));
        SelectedSquadData = squadWidget.SquadData;
    }
}
