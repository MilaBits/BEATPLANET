using UnityEngine;

public class Node : MonoBehaviour {
    public Player owner;

    public float ProductionRate = 1;
    public float UnitCount;


    public bool debug;

    private MeshRenderer meshRenderer;

    private void Start() {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.transform.localScale = new Vector3(ProductionRate, ProductionRate, ProductionRate);
    }

    private void Update() {
        if (owner) {
            UnitCount += ProductionRate * Time.deltaTime;
        }
    }
}