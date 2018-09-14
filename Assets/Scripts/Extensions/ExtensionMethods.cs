using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
    public static Vector3 DegreeToVector(this float degree) {
        float radians = degree * (Mathf.PI / 180);
        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        return degreeVector;
    }
}