using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class Ring {
    [BoxGroup("Ring"), InlineEditor] public Planet planet;

    [BoxGroup("Ring"), OnValueChanged("UpdatePatternList"), LabelText("Sectors"), EnumToggleButtons]
    public SectorNumber SectorSelect;

    [HideInInspector] public int SectorCount;

    [BoxGroup("Ring"), Range(0, 15)] public int startSector;
    [BoxGroup("Ring")] public float speed;

    [BoxGroup("Ring"), EnumToggleButtons]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowPaging = false)]
    public List<SectorState> pattern;

    public List<Transform> SectorTransforms;
    private int planetSector;

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

    private Vector3 DegreeToVector(float degree) {
        float radians = degree * (Mathf.PI / 180);
        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        return degreeVector;
    }

    public void Next() {
        if (planetSector == SectorCount - 1) planetSector = 0;
        else planetSector++;

        planet.NextSector(SectorTransforms[planetSector].GetComponent<Renderer>().bounds.center);
    }

    public enum SectorNumber {
        Four = 3,
        Eight = 7,
        Sixteen = 15
    }
}