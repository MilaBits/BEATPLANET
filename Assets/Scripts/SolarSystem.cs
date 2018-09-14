using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class SolarSystem : MonoBehaviour {
    [BoxGroup("System")] public int Units;

    [BoxGroup("Rings")] public float CenterSpacing;
    [BoxGroup("Rings")] public float RingSpacing;
    [BoxGroup("Rings")] public List<Ring> Rings;


    [BoxGroup("Materials"), LabelText("Blank")]
    public Material BlankSectorMaterial;

    [BoxGroup("Materials"), LabelText("Tap")]
    public Material TapSectorMaterial;

    [BoxGroup("Materials"), LabelText("Slide")]
    public Material SlideSectorMaterial;

    private List<GameObject> Sectors;

    private void Start() {
        for (var i = 0; i < Rings.Count; i++) {
            Ring ring = Rings[i];
            ring.planet = Instantiate(ring.planet, transform);
            ring.planet.name = "Planet " + i;

            Ray ray = new Ray(transform.position, ring.StartDirection());
            ring.planet.transform.localPosition = ray.GetPoint(CenterSpacing + RingSpacing * (float)i);
        }

        GenerateSectorMeshes();
    }

    private void GenerateSectorMeshes() {
        GameObject defaultSector = new GameObject();
        defaultSector.AddComponent<MeshRenderer>();
        defaultSector.AddComponent<MeshFilter>();

        for (var ringIndex = 0; ringIndex < Rings.Count; ringIndex++) {
            Ring ring = Rings[ringIndex];

            float dist = CenterSpacing + RingSpacing * ringIndex;

            for (int i = 0; i < ring.SectorCount; i++) {
                GameObject sector = Instantiate(defaultSector, transform);
                sector.name = String.Format("Sector {0}, {1}", ringIndex, i);
                Mesh mesh = sector.GetComponent<MeshFilter>().mesh;

                Vector3[] vertices = new Vector3[4];

                float sectorSize = -360f / ring.SectorCount;

                float leftdeg = (sectorSize * i);
                float rightdeg = (sectorSize * (i + 1));

                if (ring.pattern[i] != Ring.SectorState.Slide) {
                    leftdeg--;
                    rightdeg++;
                }
                
                Ray leftRay = new Ray(transform.localPosition, leftdeg.DegreeToVector());
                Ray rightRay = new Ray(transform.localPosition, rightdeg.DegreeToVector());

                vertices[0] = leftRay.GetPoint(dist + RingSpacing / 2); //topleft
                vertices[1] = rightRay.GetPoint(dist + RingSpacing / 2); //topright
                vertices[2] = leftRay.GetPoint(dist - RingSpacing / 2); //bottomleft
                vertices[3] = rightRay.GetPoint(dist - RingSpacing / 2); //bottomright

                mesh.name = "Sector Mesh";
                mesh.vertices = vertices;
                mesh.triangles = new[] {0, 1, 3, 0, 3, 2};
//                mesh.normals = new[] {Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward};
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();

                MeshRenderer renderer = sector.GetComponent<MeshRenderer>();
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;

                switch (ring.pattern[i]) {
                    case Ring.SectorState.Off:
                        renderer.material = BlankSectorMaterial;
                        break;
                    case Ring.SectorState.Tap:
                        renderer.material = TapSectorMaterial;
                        break;
                    case Ring.SectorState.Slide:
                        renderer.material = SlideSectorMaterial;
                        break;
                }
            }
        }
    }

    private void Update() {
        foreach (Ring ring in Rings) {
            OrbitPlanet(ring);
        }
    }

    private void OrbitPlanet(Ring ring) {
        ring.planet.transform.RotateAround(transform.position, Vector3.up, ring.speed * Time.deltaTime);
    }
}