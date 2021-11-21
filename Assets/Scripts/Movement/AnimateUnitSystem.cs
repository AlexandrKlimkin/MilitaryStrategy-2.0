using Leopotam.Ecs;
using UnityEngine;

namespace Movement
{
    public class AnimateUnitSystem : IEcsRunSystem
    {
        private EcsFilter<AnimatedUnitComponent, MovableComponent> _AnimatedFilter;
        
        public void Run()
        {
            foreach (var i in _AnimatedFilter)
            {
                var animatedUnit = _AnimatedFilter.Get1(i);
                var movable = _AnimatedFilter.Get2(i);
                var curVel = movable.NavMeshAgent.velocity.magnitude;
                animatedUnit.Animator.SetFloat("Speed", curVel);
                // animatedUnit.Animator
            }
        }
    }
}