﻿using Leopotam.Ecs;

namespace Movement
{
    public class HealthSystem : IEcsRunSystem, IEcsInitSystem
    {
        public void Init()
        {
            // healthComp.Alive = true;
            // healthComp.MaxHealth = unitData.MaxHealth;
            // healthComp.Health = healthComp.MaxHealth;
        }
        
        public void Run()
        {
            
        }
    }
}