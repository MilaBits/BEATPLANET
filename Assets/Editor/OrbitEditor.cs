using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Orbit))]
public class OrbitEditor : Editor {
//    private void OnSceneGUI() {
//        Orbit orbit = (Orbit) target;
//
//        Vector3 heading = orbit.OrbitAround.transform.position - orbit.transform.position;
//        float distance = heading.magnitude - orbit.Radius;
//
//        Ray ray = new Ray(orbit.transform.position, heading);
//
//        Handles.DrawDottedLine(orbit.transform.position, ray.GetPoint(distance), 5f);
//        Handles.DrawWireArc(orbit.OrbitAround.transform.position, Vector3.up, Vector3.forward, 360, orbit.Radius);
//    }
    
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(Orbit orbit, GizmoType gizmoType)
    {
        if (!orbit.OrbitAround) return;
        
        bool selected = gizmoType == (GizmoType.Active | GizmoType.InSelectionHierarchy | GizmoType.Selected);
        
        if (selected) {
            Handles.color = Color.yellow;
        }
        else {
            Handles.color = Color.white;
        }

        Vector3 heading = orbit.OrbitAround.transform.position - orbit.transform.position;
        float distance = heading.magnitude - orbit.Radius;

        Ray ray = new Ray(orbit.transform.position, heading);

        Handles.DrawDottedLine(orbit.transform.position, ray.GetPoint(distance), 5f);
        Handles.DrawWireArc(orbit.OrbitAround.transform.position, Vector3.up, Vector3.forward, 360, orbit.Radius);
    }
}