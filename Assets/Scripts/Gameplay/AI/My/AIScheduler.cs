using UnityEngine;


public class AIScheduler : Scheduler<AIScheduler, AIController>
{
    public float UnitsPerFrame;
    
    protected override bool MaintainConstantLoadAmmount { get { return true; } }

    protected override float ObjectsPerFrame {
        get {
            return UnitsPerFrame; // TODO: Enable Scheduler only after game start
        }
    }

    protected override void UpdateObject(AIController target) {
        target.UpdateAI();
    }
}

