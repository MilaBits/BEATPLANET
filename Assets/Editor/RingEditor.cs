using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ring))]
public class RingEditor : OdinEditor {
    public static float dividerHeight = 0.5f;

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(Ring ring, GizmoType gizmoType) {
        dividerHeight = ring.system.RingSpacing / 2;

        bool selected = gizmoType == (GizmoType.Active | GizmoType.InSelectionHierarchy | GizmoType.Selected);
        if (selected) {
            dividerHeight -= 0.05f;
            Handles.color = Color.yellow;
        }

        float radius = ring.system.CenterSpacing + ring.system.RingSpacing * ring.transform.GetSiblingIndex();

        Handles.DrawWireArc(ring.system.transform.position, Vector3.up, Vector3.forward, 360, radius - dividerHeight);
        Handles.DrawWireArc(ring.system.transform.position, Vector3.up, Vector3.forward, 360, radius + dividerHeight);

        if (ring.SectorCount.Equals(0)) return;

        float sectorSize = -360f / ring.SectorCount;
        for (int j = 0; j < ring.SectorCount; j++) {
            float sectorStart = sectorSize * j;

            Vector3 direction = sectorStart.DegreeToVector();
            Ray ray = new Ray(ring.system.transform.position, direction);

            Handles.DrawLine(ray.GetPoint(radius - dividerHeight), ray.GetPoint(radius + dividerHeight));

            if (ring.pattern[j] == SectorState.Tap) {
                direction = (sectorSize * j + sectorSize / 2).DegreeToVector();
                ray = new Ray(ring.system.transform.position, direction);
                Handles.RectangleHandleCap(
                    0,
                    ray.GetPoint(radius),
                    Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0),
                    .2f,
                    EventType.Repaint
                );
                continue;
            }

            if (ring.pattern[j] == SectorState.Slide) {
                ray = new Ray(ring.system.transform.position, (sectorSize * j + sectorSize / 2f).DegreeToVector());
                Handles.DrawWireArc(ray.GetPoint(radius), Vector3.up, Vector3.forward, 360, .2f);
            }
        }
    }

    private void Awake() {
        Ring ring = (Ring) target;

        if (!ring.system) ring.system = ring.transform.parent.parent.GetComponent<SolarSystem>();
    }
}