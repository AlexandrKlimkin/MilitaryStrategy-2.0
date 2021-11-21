using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnSystem : MonoBehaviour {

    private List<Transform> _SpawnPoints;
    public List<GameObject> SpawnObjects;

    private void Awake() {
        _SpawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i);
            if (child.name.Contains("SpawnPoint")) {
                _SpawnPoints.Add(child);
            }
            _SpawnPoints.Add(transform.GetChild(i));
        }
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SpawnObject(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SpawnObject(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SpawnObject(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SpawnObject(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SpawnObject(4);
        }
    }

    private void SpawnObject(int index) {
        if (index >= SpawnObjects.Count)
            return;
        if (_SpawnPoints == null)
            return;
        var objTemplate = SpawnObjects[index];
        if (objTemplate == null)
            return;
        var transformIndex = Random.Range(0, _SpawnPoints.Count);
        var spawnTransform = _SpawnPoints[transformIndex];
        var randVec = Random.insideUnitCircle * 5;
        var spawnPos = spawnTransform.position + new Vector3(randVec.x, 0, randVec.y);
        Instantiate(objTemplate, spawnPos, spawnTransform.rotation);
    }
}
