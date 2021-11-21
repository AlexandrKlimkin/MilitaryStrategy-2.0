using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBig : Unit {

    public GameObject LittleSpiderPrefab;
    private Transform _SpawnTransformsHost;
    private List<Transform> _SpawnTransforms;

    protected override void Awake() {
        base.Awake();
        _SpawnTransformsHost = transform.Find("SpawnTransformsHost");
        _SpawnTransforms = new List<Transform>(_SpawnTransformsHost.childCount);
        for (int i = 0; i < _SpawnTransformsHost.childCount; i++) {
            _SpawnTransforms.Add(_SpawnTransformsHost.GetChild(i));
        }
    }

    protected override void Start () {
        base.Start();
        OnDeath += SpawnLitteSpiders;
	}

    private void SpawnLitteSpiders() {
        foreach (var tr in _SpawnTransforms) {
            Instantiate(LittleSpiderPrefab, tr.position, tr.rotation);
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        OnDeath -= SpawnLitteSpiders;
    }
}
