using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SolarSystem : MonoBehaviour {
    [BoxGroup("System")] public int Score;

    [BoxGroup("Rings")] public float CenterSpacing;
    [BoxGroup("Rings")] public float RingSpacing;

    private List<GameObject> Sectors;

    public Text ScoreText;
    
    private void Update() {
        ScoreText.text = Score.ToString();
    }
}