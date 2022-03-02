using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SquadsSpawner : MonoBehaviour
{
    public GameObject UnitPrefab;
    [Header("SIZE")]
    [Range(1, 100)]
    public int Width;
    [Range(1, 100)]
    public int Length;
    [Range(1, 100)] 
    public int Height;
    
    [Header("PADDING")]
    [Range(0, 100)]
    public float widthPadding;
    [Range(0, 100)]
    public float lengthPadding;
    [Range(0, 100)] 
    public float heightPadding;
    
    [Header("ROTATION")]
    public Vector3 Rotation;
    public bool RandomRotation;
    public Vector2 RandomXRotation;
    public Vector2 RandomYRotation;
    public Vector2 RandomZRotation;

    [Header("SCALE")]
    public Vector3 Scale;
    public bool RandomScale;
    public Vector2 RandomXScale;
    public Vector2 RandomYScale;
    public Vector2 RandomZScale;
    
    [Header("OFFSET")] 
    public bool CustomOffset;
    [Button]
    public bool _SetCenterOffset;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    [Header("OTHER")] 
    public bool SpawnOnStart;
    public bool UseParentObject;
    public bool UseDots;
    [Header("UNIT SETTINGS")] 
    public int TeamIndex;
    [Header("RIGIDBODY SETTINGS")]
    public bool AutoMass;


    private Entity _objEntity;
    private EntityManager _entityManager;
    private BlobAssetStore _blobAssetStore;
    private GameObjectConversionSettings _gameObjectConversionSettings;

    private void Start()
    {
        if (UseDots)
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _blobAssetStore = new BlobAssetStore();
            _gameObjectConversionSettings =
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
            _objEntity =
                GameObjectConversionUtility.ConvertGameObjectHierarchy(UnitPrefab, _gameObjectConversionSettings);
        }

        if (SpawnOnStart)
            OnSpawn();
    }


    [Space]
    [Button]
    public bool _Spawn;

    public void OnSpawn()
    {
        SpawnSquad(UnitPrefab, Width, Length, Height, widthPadding, lengthPadding, heightPadding);
    }

    public void SpawnSquad(GameObject prefab, int width, int length, int height, float widthPadding, float lengthPadding, float heightPadding)
    {
        if (prefab == null)
        {
            Debug.LogError("Unit prefab is null!");
            return;
        }
        var squadRoot = new GameObject($"{prefab.name}_Squad_{width}x{length}x{height}_Team_{TeamIndex}");
        squadRoot.transform.position = transform.position;
        squadRoot.transform.rotation = transform.rotation;

        if (!CustomOffset)
            OnSetCenterOffset();
        
        var rootPos = squadRoot.transform.position;
        
        var units = new List<GameObject>();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    var pos = new Vector3(i * widthPadding - offsetX, k * heightPadding - offsetY, j * lengthPadding - offsetZ);
                    if (UseDots)
                    {
                        var entity = _entityManager.Instantiate(_objEntity);
                        Translation trans = new Translation()
                        {
                            Value = new float3(i * widthPadding - offsetX + rootPos.x, k * heightPadding - offsetY + rootPos.y, j * lengthPadding - offsetZ + rootPos.z)
                        };
                        _entityManager.SetComponentData(entity, trans);
                    }
                    else
                    {
                        var unit = Instantiate(prefab, squadRoot.transform);
                        unit.transform.localPosition = pos;

                        unit.transform.localRotation = GetRotation();
                        unit.transform.localScale = GetScale();
                        units.Add(unit);

                        ApplyUnitSettings(unit);
                        ApplyRigidbodySettings(unit);
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        _blobAssetStore?.Dispose();
    }

    private Quaternion GetRotation()
    {
        if (RandomRotation)
        {
            var randX = Random.Range(RandomXRotation.x, RandomXRotation.y);
            var randY = Random.Range(RandomYRotation.x, RandomYRotation.y);
            var randZ = Random.Range(RandomZRotation.x, RandomZRotation.y);
            return Quaternion.Euler(randX, randY, randZ);
        }
        else
        {
            return Quaternion.Euler(Rotation);
        }
    }
    
    private Vector3 GetScale()
    {
        if (RandomScale)
        {
            var randX = Random.Range(RandomXScale.x, RandomXScale.y);
            var randY = Random.Range(RandomYScale.x, RandomYScale.y);
            var randZ = Random.Range(RandomZScale.x, RandomZScale.y);
            return new Vector3(randX, randY, randZ);
        }
        else
        {
            return Scale; 
        }
    }

    private void ApplyUnitSettings(GameObject obj)
    {
        var unit = obj.GetComponent<Unit>();
        if(unit == null)
            return;
        unit.TeamIndex = TeamIndex;
    }

    private void ApplyRigidbodySettings(GameObject obj)
    {
        if (AutoMass)
        {
            var rb = GetComponent<Rigidbody>();
            var capsule = GetComponent<CapsuleCollider>();
            if(rb == null)
                return;
            if (capsule != null)
            {
                var v = 3.14f * (Mathf.Pow((capsule.radius), 2)) * ((4 / 3) * capsule.radius + capsule.height);
                rb.mass = v;
            }
        }
    }
    
    public void OnSetCenterOffset()
    {
        offsetX = (Width * widthPadding) / 2f;
        offsetZ = (Length * lengthPadding - 1) / 2f;
        offsetY = (Height * heightPadding - 1) / 2f;
    }
}