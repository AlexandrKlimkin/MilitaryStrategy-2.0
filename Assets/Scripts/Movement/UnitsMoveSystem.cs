using Leopotam.Ecs;
using UnityEngine;

namespace Movement
{
    public class UnitsMoveSystem : IEcsRunSystem
    {
        private EcsFilter<MovableComponent> _UnitMoveFilter;
        
        public void Run()
        {
            foreach (var i in _UnitMoveFilter)
            {
                var movableComponent = _UnitMoveFilter.Get1(i);
                
                var newTargetPoint = new Vector3(100f, 0f, 0f);

                if (movableComponent.TargetPoint != newTargetPoint)
                {
                    movableComponent.TargetPoint = newTargetPoint;
                    movableComponent.NavMeshAgent.SetDestination(movableComponent.TargetPoint);
                    movableComponent.NavMeshAgent.isStopped = false;
                }
            }    
        }
    }
}