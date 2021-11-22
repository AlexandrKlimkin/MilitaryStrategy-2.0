using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class GameInitSystem : IEcsInitSystem
    {
        private EcsWorld _World = null;
        
        public void Init()
        {
            CreateUnit();
        }

        private void CreateUnit()
        {
            var unit = _World.NewEntity();
            var unitSpawn = GameObject.FindGameObjectWithTag("UnitSpawn");
            ref var unityComp = ref unit.Get<UnityObjComponent>();
            ref var unitInfoComp = ref unit.Get<UnitInfoComponent>();
            ref var movableComp = ref unit.Get<MovableComponent>();
            ref var animatorComp = ref unit.Get<AnimatedUnitComponent>();
            ref var healthComp = ref unit.Get<HealthComponent>();

            var unitData = UnitInitData.LoadFromAssets();

            var pos = unitSpawn == null ? Vector3.zero : unitSpawn.transform.position; 
            var rot = unitSpawn == null ? Quaternion.identity : unitSpawn.transform.rotation; 
            
            var spawnedUnit = GameObject.Instantiate(unitData.UnitPrefab, pos, rot);

            unityComp.GameObject = spawnedUnit.gameObject;
            unityComp.Transform = spawnedUnit.transform;
            
            movableComp.Transform = spawnedUnit.transform;
            movableComp.NavMeshAgent = spawnedUnit.GetComponent<NavMeshAgent>();
            movableComp.MoveSpeed = unitData.DefaultSpeed;
            movableComp.CanMove = true;

            animatorComp.Animator = spawnedUnit.GetComponent<Animator>();
        }
    }
}