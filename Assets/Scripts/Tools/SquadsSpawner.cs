using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadsSpawner : MonoBehaviour
{
    public GameObject UnitPrefab;
    [Header("Size")]
    [Range(1, 100)]
    public int Width;
    [Range(1, 100)]
    public int Length;
    [Header("Padding")]
    [Range(1, 100)]
    public int widthPadding;
    [Range(1, 100)]
    public int lengthPadding;
    [Button]
    public bool _Spawn;

    public void OnSpawn()
    {
        SpawnSquad(UnitPrefab, Width, Length, widthPadding, lengthPadding);
    }

    public void SpawnSquad(GameObject prefab, int width, int length, float widthPadding, float lengthPadding)
    {
        if (prefab == null)
        {
            Debug.LogError("Unit prefab is null!");
            return;
        }
        var squadRoot = new GameObject($"{prefab.name}_Squad_{width}x{length}");
        squadRoot.transform.position = transform.position;
        squadRoot.transform.rotation = transform.rotation;

        var units = new List<GameObject>();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var pos = new Vector3(i * widthPadding, 0, j * lengthPadding);
                var unit = Instantiate(prefab, squadRoot.transform);
                unit.transform.localPosition = pos;
                unit.transform.localRotation = Quaternion.identity;
                units.Add(unit);
            }
        }
        SetAnimationInstancing(units);
    }

    private void SetAnimationInstancing(List<GameObject> units)
    {
        if(units.Count < 1)
            return;
        foreach (var unit in units)
        {
            var animInst = unit.GetComponent<AnimationInstancing.AnimationInstancing>();
            if(animInst == null)
                return;
            animInst.prototype = UnitPrefab;
        }
    }
}
