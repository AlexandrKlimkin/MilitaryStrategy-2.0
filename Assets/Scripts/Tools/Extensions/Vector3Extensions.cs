using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions {
    public static Vector2 ToVector2XY(this Vector3 vector) {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2 ToVector2XZ(this Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }
}
