using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
    public GameObject OrbitAround;
    [HideInInspector] public Vector3 Axis = Vector3.up;
    [HideInInspector] public Vector3 DesiredPosition;
    public float Radius = 2.0f;
    public float RotationSpeed = 80.0f;

    void Start() {
        if (!OrbitAround) return;
        
        Vector3 heading = OrbitAround.transform.position - transform.position;
        float distance = heading.magnitude - Radius;
        Ray ray = new Ray(transform.position, heading);
        
        transform.position = ray.GetPoint(distance);
        Radius = 2.0f;
    }

    void Update() {
        if (!OrbitAround) return;
        
        transform.RotateAround(OrbitAround.transform.position, Axis, RotationSpeed * Time.deltaTime);
    }
}