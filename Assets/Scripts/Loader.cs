using System;
using Leopotam.Ecs;
using Units;
using UnityEngine;

namespace DefaultNamespace
{
    public class Loader : MonoBehaviour
    {
        private EcsWorld _World;
        private EcsSystems Systems;

        private void Start()
        {
            _World = new EcsWorld();
            Systems = new EcsSystems(_World);

            Systems.Add(new GameInitSystem());
            Systems.Add(new UnitsMoveSystem());
            Systems.Add(new AnimateUnitSystem());
            Systems.Init();
        }

        private void Update()
        {
            Systems.Run();
        }

        private void OnDestroy()
        {
            Systems.Destroy();
            _World.Destroy();
        }
    }
}