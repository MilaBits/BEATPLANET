using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SolarSystem : MonoBehaviour {
    
    [BoxGroup("System")]
    public int Units;

    [BoxGroup("Rings")] public float CenterSpacing;
    [BoxGroup("Rings")] public float RingSpacing;
    [BoxGroup("Rings")] public List<Ring> rings;

    private void Start() {
        for (var i = 0; i < rings.Count; i++) {
            Ring ring = rings[i];
            ring.planet = Instantiate(ring.planet, transform);
            
            Ray ray = new Ray(transform.position, ring.StartDirection());
            ring.planet.transform.localPosition = ray.GetPoint(CenterSpacing + RingSpacing * i);
        }
    }

    private void Update() {
        foreach (Ring ring in rings) {
            OrbitPlanet(ring);
        }
    }

    private void OrbitPlanet(Ring ring) {
        ring.planet.transform.RotateAround(transform.position, Vector3.up, ring.speed * Time.deltaTime);
    }
}    