using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class SolarSystem : MonoBehaviour {
    [BoxGroup("System")] public int Units;
    [BoxGroup("System")] public SingleUnityLayer SectorLayer;

    [BoxGroup("Music")] public int Bpm;
    [BoxGroup("Music")] public float PassedBeats;

    private float nextBeatTime = 0.0f;

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
            ring.planet.transform.position = ray.GetPoint(CenterSpacing + RingSpacing * (float) i);
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

            List<Transform> meshTransforms = new List<Transform>();
            for (int i = 0; i < ring.SectorCount; i++) {
                GameObject sector = Instantiate(defaultSector, transform);
                sector.name = String.Format("Sector {0}, {1}", ringIndex, i);
                Mesh mesh = sector.GetComponent<MeshFilter>().mesh;

                Vector3[] vertices = new Vector3[4];

                float sectorSize = -360f / ring.SectorCount;

                float leftdeg = (sectorSize * i);
                float rightdeg = (sectorSize * (i + 1));

                if (ring.pattern[i] != SectorState.Slide) {
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
                mesh.RecalculateBounds();
                mesh.RecalculateNormals();

                MeshRenderer renderer = sector.GetComponent<MeshRenderer>();
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;

                Sector SectorComponent = sector.AddComponent<Sector>();
                switch (ring.pattern[i]) {
                    case SectorState.Off:
                        SectorComponent.SectorState = SectorState.Off;
                        renderer.material = BlankSectorMaterial;
                        break;
                    case SectorState.Tap:
                        SectorComponent.SectorState = SectorState.Tap;
                        renderer.material = TapSectorMaterial;
                        break;
                    case SectorState.Slide:
                        SectorComponent.SectorState = SectorState.Slide;
                        renderer.material = SlideSectorMaterial;
                        break;
                }


                sector.AddComponent<MeshCollider>();
                sector.layer = 10;

                meshTransforms.Add(sector.transform);
            }

            ring.SectorTransforms = meshTransforms;
        }
    }

    private void Update() {
        if (Time.time > nextBeatTime) {
            nextBeatTime += 60f / Bpm;
            PassedBeats++;

            foreach (Ring ring in Rings) {
                switch (ring.SectorSelect) {
                    case Ring.SectorNumber.Four:
                        if (Mathf.Floor(PassedBeats) % 4 == 1) ring.Next();
                        break;
                    case Ring.SectorNumber.Eight:
                        if (Mathf.Floor(PassedBeats) % 2 == 1) ring.Next();
                        break;
                    case Ring.SectorNumber.Sixteen:
                        ring.Next();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void OrbitPlanet(Ring ring) {
        ring.planet.transform.RotateAround(transform.position, Vector3.up, ring.speed * Time.deltaTime);
    }
}