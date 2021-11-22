using Leopotam.Ecs;

namespace Units
{
    public class AttackSystem : IEcsRunSystem
    {
        private EcsFilter<AttackComponent, UnitInfoComponent> _UnitMoveFilter;
        
        public void Run()
        {
            foreach (var i in _UnitMoveFilter)
            {
                var attackComponent = _UnitMoveFilter.Get1(i);
                var unitInfoComponent = _UnitMoveFilter.Get2(i);
            }
        }
    }
}