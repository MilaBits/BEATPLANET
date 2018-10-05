using UnityEngine;

public class Sector : MonoBehaviour {
    public SectorState SectorState;

    public Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }
}