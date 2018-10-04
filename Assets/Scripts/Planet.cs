using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour {
    public float stepTime = 0.1f;
    public float TapTime = 0.1f;
    public float SlideTime = 0.3f;
    public LayerMask SectorMask;

    public bool slide = false;

    float currentTime = 0.0f;
    float percentage;

    private Vector3 start;
    private Vector3 moveTarget;

    private IEnumerator coroutine;

    private bool running;

    private SphereCollider collider;

    private void Start() {
        collider = GetComponent<SphereCollider>();
    }

    public void NextSector(Transform target) {
        start = transform.position;
        moveTarget = target.GetComponent<Renderer>().bounds.center;
        currentTime = 0f;

        
//        if (target.GetComponent<Sector>().SectorState == SectorState.Slide) {
//            stepTime = SlideTime;
//        }
//        else {
//            stepTime = TapTime;
//        }

        if (coroutine == null) {
            coroutine = MoveToNextSector(target);
        }

        if (!running) {
            running = true;
            StartCoroutine(coroutine);
            return;
        }
    }

    public void StopMoving() {
        running = false;
    }

    IEnumerator MoveToNextSector(Transform target) {
        while (running) {
            percentage = currentTime / stepTime;
            transform.position = Vector3.Lerp(start, moveTarget, percentage);

            if (percentage > .95f) transform.position = moveTarget;
            currentTime += Time.deltaTime;
            yield return 0;
        }
    }

    public ClosestSectorData GetClosestSector() {
        ClosestSectorData closest = new ClosestSectorData();
        closest.distance = 1f;
        foreach (Collider sector in Physics.OverlapSphere(transform.position, collider.radius, SectorMask)) {
            float dist = Vector3.Distance(sector.bounds.center, transform.position);
            if (Vector3.Distance(sector.bounds.center, transform.position) < closest.distance) {
                closest = new ClosestSectorData {distance = dist, sector = sector.GetComponent<Sector>()};
            }
        }

        return closest;
    }

    public struct ClosestSectorData {
        public float distance;
        public Sector sector;
    }
}