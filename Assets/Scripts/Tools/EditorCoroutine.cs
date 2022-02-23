using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorUtils {
    // public class EditorCoroutine {
    //
    //     private readonly IEnumerator _Routine;
    //
    //     public EditorCoroutine(IEnumerator routine) {
    //         _Routine = routine;
    //     }
    //
    //     public static EditorCoroutine Start(IEnumerator _routine) {
    //         EditorCoroutine coroutine = new EditorCoroutine(_routine);
    //         coroutine.Start();
    //         return coroutine;
    //     }
    //
    //     private void Start() {
    //         EditorApplication.update += Update;
    //     }
    //
    //     public void Stop() {
    //         EditorApplication.update -= Update;
    //     }
    //
    //     private void Update() {
    //         if (!_Routine.MoveNext()) {
    //             Stop();
    //         }
    //     }
    // }
}