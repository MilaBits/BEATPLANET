using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SolarSystem : MonoBehaviour {
    [BoxGroup("System")] public int Units;

    [BoxGroup("Rings")] public float CenterSpacing;
    [BoxGroup("Rings")] public float RingSpacing;

    private List<GameObject> Sectors;

}