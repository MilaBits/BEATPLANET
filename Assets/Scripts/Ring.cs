using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class Ring {
    [BoxGroup("Ring")] public Planet planet;
    [BoxGroup("Ring"), OnValueChanged("UpdatePatternList"), LabelText("Sectors"), EnumToggleButtons]
    public SectorNumber SectorSelect;
    
    [HideInInspector]
    public int SectorCount;

    [BoxGroup("Ring"), Range(0, 15)] public int startSector;
    [BoxGroup("Ring")] public float speed;
    [BoxGroup("Ring"), EnumToggleButtons] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowPaging = false)] public List<SectorState> pattern;

    public Vector3 StartDirection() {
        float sectorSize = 360 / SectorCount;

        float degree = sectorSize * startSector;
        float radians = degree * (Mathf.PI / 180);
        Vector3 degreeVector = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        return degreeVector;
    }

    private void UpdatePatternList() {
        SectorCount = (int)SectorSelect;
        
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
        // Move plane to next sector
        
        
    }
    
    public enum SectorNumber {
        Four = 4,
        Eight = 8,
        Sixteen = 16
    }

    public enum SectorState {
        Off,
        Tap,
        Slide
    }
}