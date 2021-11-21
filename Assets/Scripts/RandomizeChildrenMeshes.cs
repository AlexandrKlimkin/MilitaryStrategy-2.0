using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class RandomizeChildrenMeshes : MonoBehaviour {

    public GameObject ReplaceableMeshObject;
    public List<GameObject> RandomObjects;

    private Mesh _ReplacableMesh;

    public bool REMEMBER;
    public bool RANDOMIZE;
    public bool REVERT;

    private List<GameObject> _randomizedObjects;

    private void Start() {
        
    }

    void Update () {
        if (REMEMBER) {
            REMEMBER = false;
            RememberMeshFilters();
        }

        if (RANDOMIZE) {
            RANDOMIZE = false;
            RandomizeChildren();
        }

        if (REVERT) {
            REVERT = false;
            RevertChanges();
        }
	}

    private void RememberMeshFilters() {
        if (ReplaceableMeshObject == null || RandomObjects == null)
            return;
        var rObjMeshFilter = ReplaceableMeshObject.GetComponent<MeshFilter>();
        var meshFilters = new List<MeshFilter>();
        GetComponentsInChildren(meshFilters);
        _randomizedObjects = new List<GameObject>();

        foreach(var mFilter in meshFilters) {
            if(mFilter.sharedMesh.vertexCount == rObjMeshFilter.sharedMesh.vertexCount) {
                _randomizedObjects.Add(mFilter.gameObject);
            }
        }
    }

    private void RandomizeChildren() {
        if (ReplaceableMeshObject == null || RandomObjects == null)
            return;
        var count = RandomObjects.Count;
        _randomizedObjects.ForEach( _ => {
            var rand = Random.Range(0, count);
            var localPos = _.transform.localPosition;
            var localRot = _.transform.rotation;
            var localScale = _.transform.localScale;

            var newObj = Instantiate(RandomObjects[rand], _.transform.parent);
            newObj.transform.localPosition = localPos;
            newObj.transform.localRotation = localRot;
            newObj.transform.localScale = localScale;
            DestroyImmediate(_);
            });
    }

    private void RevertChanges() {
        //if (ReplaceableMeshObject == null || RandomObjects == null)
        //    return;
        //_ReplacableMesh = ReplaceableMeshObject.GetComponent<MeshFilter>().mesh;
        //_randomizedObjects.ForEach(_ => _.sharedMesh = _ReplacableMesh);
    }
}
