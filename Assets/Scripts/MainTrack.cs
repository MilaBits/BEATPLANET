using UnityEngine;
using UnityEngine.Events;

public class MainTrack : MonoBehaviour {
    public int Bpm;
    public float BeatCount;

    public UnityEvent Beat;

    void Update() {
        BeatCount += Time.deltaTime * (Bpm / 60);
        
        Debug.Log("Beat Sent");
        Beat.Invoke();
    }
}