using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class Ring {
    [BoxGroup("Ring")] public Planet planet;

    [BoxGroup("Ring"), OnValueChanged("UpdatePatternList")] [Range(1, 16)]
    public int sectors;

    [BoxGroup("Ring"), Range(1, 16)] public int startSector;
    [BoxGroup("Ring")] public float speed;
    [BoxGroup("Ring"), EnumToggleButtons] public List<SectorState> pattern;

    public Vector3 StartDirection() {
        float sectorSize = 360 / sectors;

        float degree = sectorSize * startSector;
        float radians = degree * (Mathf.PI / 180);
        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        return degreeVector;
    }

    private void UpdatePatternList() {
        if (pattern.Count.Equals(sectors)) return;
        if (pattern.Count < sectors) {
            List<SectorState> newList = new SectorState[sectors].ToList();
            for (var i = 0; i < pattern.Count; i++) {
                SectorState sectorState = pattern[i];
                newList[i] = pattern[i];
            }

            pattern = newList;
        }

        if (pattern.Count > sectors) {
            List<SectorState> newList = new SectorState[sectors].ToList();
            for (int i = 0; i < sectors; i++) {
                newList[i] = pattern[i];
            }

            pattern = newList;
        }
    }

    public enum SectorState {
        Off,
        Tap,
        Slide
    }
}