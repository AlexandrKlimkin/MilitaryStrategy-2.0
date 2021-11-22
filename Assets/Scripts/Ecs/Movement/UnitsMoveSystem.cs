using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class UnitsMoveSystem : IEcsRunSystem
    {
        private EcsFilter<MovableComponent> _UnitMoveFilter;
        
        public void Run()
        {
            foreach (var i in _UnitMoveFilter)
            {
                ref var movableComponent = ref _UnitMoveFilter.Get1(i);
                movableComponent.NavMeshAgent.speed = movableComponent.MoveSpeed;
                
                if (movableComponent.TargetPoint.HasValue && movableComponent.CanMove)
                {
                    movableComponent.NavMeshAgent.SetDestination(movableComponent.TargetPoint.Value);
                    movableComponent.NavMeshAgent.isStopped = false;
                }
            }    
        }
    }
}