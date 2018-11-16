using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseOnBeat : MonoBehaviour {
    private BeatObserver beatObserver;

    private Animator animator;

    // Use this for initialization
    void Start() {
        if (GetComponent<BeatObserver>() != null) {
            beatObserver = GetComponent<BeatObserver>();
        }
        else {
            beatObserver = GetComponentInParent<BeatObserver>();
        }

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (beatObserver.beatMask != 0) {
            animator.SetTrigger("Pulse");
        }
    }
}