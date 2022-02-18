/*
THIS FILE IS PART OF Animation Instancing PROJECT
AnimationInstancing.cs - The core part of the Animation Instancing library

©2017 Jin Xiaoyu. All Rights Reserved.
*/

using UnityEngine;

public class AnimationInstancingReferenceHolder<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;

    public static T Instance {
        get {
            if (_Instance != null)
                return _Instance;
            var singleton = new GameObject();
            _Instance = singleton.AddComponent<T>();
            singleton.name = "(animation instancing) " + typeof(T).ToString();
            //DontDestroyOnLoad(singleton);
            return _Instance;
        }
    }

    private static bool applicationIsQuitting;

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }

    public static bool IsDestroy()
    {
        return applicationIsQuitting;
    }
}
