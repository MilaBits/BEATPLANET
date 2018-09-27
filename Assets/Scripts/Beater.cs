using UnityEngine;

public class Beater : MonoBehaviour {

	public MainTrack MainTrack;

	public int PulseEveryXBeats;

	public Animator pulse;
	

	// Use this for initialization
	void Start () {
		MainTrack = FindObjectOfType<MainTrack>();
		
		MainTrack.Beat.AddListener(HandleBeat);
	}

	private void HandleBeat() {
		if (Mathf.Round(MainTrack.BeatCount) % 3 != 1) return;
		
		Debug.Log("Beat Received");
		
		pulse.SetTrigger("Pulse");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
