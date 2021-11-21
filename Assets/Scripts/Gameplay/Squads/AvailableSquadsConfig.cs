using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AvailableSquadsConfig", menuName = "Config/AvailableSquadsConfig", order = 0)]
public class AvailableSquadsConfig : ScriptableObject
{
    public string UnitName;
    public List<SquadData> AvaliableSquads;

    private void Awake() {
        foreach(var squad in AvaliableSquads) {
            squad.UnitName = UnitName;
            squad.UnitsCount = squad.Size.x * squad.Size.y;
        }
    }
}