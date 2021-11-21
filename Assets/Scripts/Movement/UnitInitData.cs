using UnityEngine;

namespace Movement
{
    [CreateAssetMenu]
    public class UnitInitData : ScriptableObject
    {
        public GameObject UnitPrefab;
        public float DefaultSpeed;
        [Space] 
        public float MaxHealth;
        
        
        public static UnitInitData LoadFromAssets() => Resources.Load("UnitInitData") as UnitInitData;
    }
}