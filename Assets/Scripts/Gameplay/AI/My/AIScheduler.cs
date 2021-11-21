using UnityEngine;


public class AIScheduler : Scheduler<AIScheduler, AIController> {

    protected override bool MaintainConstantLoadAmmount { get { return true; } }

    protected override float ObjectsPerFrame {
        get {
            return 100f; // TODO: Enable Scheduler only after game start
        }
    }

    protected override void UpdateObject(AIController target) {
        target.UpdateAI();
    }
}

