using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SolarSystem)), RequireComponent(typeof(BeatObserver))]
public class Ring : MonoBehaviour {
    [BoxGroup("Ring")] public SolarSystem system;
    [BoxGroup("Ring"), InlineEditor] public Planet planet;

    [BoxGroup("Ring"), OnValueChanged("UpdatePatternList"), LabelText("Sectors"), EnumToggleButtons]
    public SectorNumber SectorSelect;

    [HideInInspector] public int SectorCount;

    [BoxGroup("Ring")] public GameObject SectorGameObject;
    [BoxGroup("Ring"), Range(0, 15)] public int startSector;

    [BoxGroup("Ring"), EnumToggleButtons]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowPaging = false)]
    public List<SectorState> pattern;

    public List<Transform> SectorTransforms;
    private int planetSector;

    [BoxGroup("Materials"), LabelText("Blank")]
    public Material BlankSectorMaterial;

    [BoxGroup("Materials"), LabelText("Tap")]
    public Material TapSectorMaterial;

    [BoxGroup("Materials"), LabelText("Slide")]
    public Material SlideSectorMaterial;

    private BeatObserver beatObserver;

    private void Start() {
        beatObserver = GetComponent<BeatObserver>();

        GenerateSectorMeshes();

        planet = Instantiate(planet, transform);
        planet.name = "Planet " + transform.GetSiblingIndex();

        Ray ray = new Ray(transform.position, StartDirection());
        planet.transform.position =
            ray.GetPoint(system.CenterSpacing + system.RingSpacing * (float) transform.GetSiblingIndex());
    }

    private void Update() {
        if (beatObserver.beatMask != 0) {
            Next();
        }
    }

    public Vector3 StartDirection() {
        planetSector = startSector;

        float sectorSize = 360 / SectorCount;

        float degree = sectorSize * startSector - sectorSize / 2;
        float radians = degree * (Mathf.PI / 180);
        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));

        return degreeVector;
    }

    private void UpdatePatternList() {
        SectorCount = (int) SectorSelect + 1;

        if (pattern.Count.Equals(SectorCount)) return;
        if (pattern.Count < SectorCount) {
            List<SectorState> newList = new SectorState[SectorCount].ToList();
            for (var i = 0; i < pattern.Count; i++) {
                SectorState sectorState = pattern[i];
                newList[i] = pattern[i];
            }

            pattern = newList;
        }

        if (pattern.Count > SectorCount) {
            List<SectorState> newList = new SectorState[SectorCount].ToList();
            for (int i = 0; i < SectorCount; i++) {
                newList[i] = pattern[i];
            }

            pattern = newList;
        }
    }

    private void GenerateSectorMeshes() {
        float dist = system.CenterSpacing + system.RingSpacing * transform.GetSiblingIndex();

        List<Transform> meshTransforms = new List<Transform>();
        for (int i = 0; i < SectorCount; i++) {
            GameObject sector = Instantiate(SectorGameObject, transform);
            sector.name = String.Format("Sector {0}, {1}", transform.GetSiblingIndex(), i);
            Mesh mesh = sector.GetComponent<MeshFilter>().mesh;

            Vector3[] vertices = new Vector3[4];

            float sectorSize = -360f / SectorCount;

            float leftdeg = (sectorSize * i);
            float rightdeg = (sectorSize * (i + 1));

            if (pattern[i] != SectorState.Slide) {
                leftdeg--;
                rightdeg++;
            }

            Ray leftRay = new Ray(transform.localPosition, leftdeg.DegreeToVector());
            Ray rightRay = new Ray(transform.localPosition, rightdeg.DegreeToVector());

            vertices[0] = leftRay.GetPoint(dist + system.RingSpacing / 2); //topleft
            vertices[1] = rightRay.GetPoint(dist + system.RingSpacing / 2); //topright
            vertices[2] = leftRay.GetPoint(dist - system.RingSpacing / 2); //bottomleft
            vertices[3] = rightRay.GetPoint(dist - system.RingSpacing / 2); //bottomright

            mesh.name = "Sector Mesh";
            mesh.vertices = vertices;
            mesh.triangles = new[] {0, 1, 3, 0, 3, 2};
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            MeshRenderer renderer = sector.GetComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            Sector SectorComponent = sector.GetComponent<Sector>();
            switch (pattern[i]) {
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

            Collider collider = sector.AddComponent<MeshCollider>();
            sector.layer = 10;

            meshTransforms.Add(sector.transform);
        }

        SectorTransforms = meshTransforms;
    }

    public void Next() {
        if (planetSector == SectorCount - 1) planetSector = 0;
        else planetSector++;
        
        //TODO: fix
        SectorTransforms[planetSector].GetComponent<Sector>().animator.SetTrigger("Pulse");
        
        planet.NextSector(SectorTransforms[planetSector]);
    }

    public enum SectorNumber {
        Four = 3,
        Eight = 7,
        Sixteen = 15
    }
}