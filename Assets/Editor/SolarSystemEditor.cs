using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SolarSystem))]
public class SolarSystemEditor : OdinEditor {
    public float dividerHeight = 0.5f;

    private void OnSceneGUI() {
        SolarSystem system = (SolarSystem) target;

        dividerHeight = system.RingSpacing / 2;

        for (int i = 0; i < system.Rings.Count; i++) {
            Ring systemRing = system.Rings[i];
            float radius = system.CenterSpacing + system.RingSpacing * i;

            Handles.DrawWireArc(system.transform.position, Vector3.up, Vector3.forward, 360, radius - dividerHeight);
            Handles.DrawWireArc(system.transform.position, Vector3.up, Vector3.forward, 360, radius + dividerHeight);

            if (systemRing.SectorCount.Equals(0)) return;

            float sectorSize = -360f / systemRing.SectorCount;
            for (int j = 0; j < systemRing.SectorCount; j++) {
                float sectorStart = sectorSize * j;

                Vector3 direction = sectorStart.DegreeToVector();
                Ray ray = new Ray(system.transform.position, direction);

                Handles.DrawLine(ray.GetPoint(radius - dividerHeight), ray.GetPoint(radius + dividerHeight));

                if (systemRing.pattern[j] == Ring.SectorState.Tap) {
                    direction = (sectorSize * j + sectorSize / 2).DegreeToVector();
                    ray = new Ray(system.transform.position, direction);
                    Handles.RectangleHandleCap(
                        0,
                        ray.GetPoint(radius),
                        Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0),
                        .2f,
                        EventType.Repaint
                    );
                    continue;
                }

                if (systemRing.pattern[j] == Ring.SectorState.Slide) {
                    ray = new Ray(system.transform.position, (sectorSize * j + sectorSize / 2f).DegreeToVector());
                    Handles.DrawWireArc(ray.GetPoint(radius), Vector3.up, Vector3.forward, 360, .2f);
                }
            }
        }
    }
}