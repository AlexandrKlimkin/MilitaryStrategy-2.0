using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameState CurrentState { get; private set; }

    void Start()
    {
        CurrentState = GameState.BattleFormation;
    }
}

public enum GameState {
    BattleFormation,
    Game,
}
