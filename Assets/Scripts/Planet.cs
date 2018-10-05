using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Planet : MonoBehaviour {
    public float stepTime = 0.1f;
    public float TapTime = 0.1f;
    public float SlideTime = 0.3f;
    public LayerMask SectorMask;

    public int TapScore = 5;

    public bool slide = false;

    float currentTime = 0.0f;
    float percentage;

    private Vector3 start;
    private Vector3 moveTarget;

    private IEnumerator coroutine;

    private bool running;

    private SphereCollider collider;

    private TrailRenderer trailRenderer;

    private Sector lastSector;
    public Sector CurrentSector;

    private Animator animator;

    private SolarSystem system;

    public bool Touchable;

    private void Start() {
        collider = GetComponent<SphereCollider>();
        trailRenderer = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();

        system = transform.parent.parent.parent.GetComponent<SolarSystem>();
    }

    public void Tap() {
        if (Touchable) {
            Touchable = false;
            system.Score += TapScore;
        }
    }

    public void NextSector(Transform target) {
        lastSector = CurrentSector;
        CurrentSector = target.GetComponent<Sector>();

        start = transform.position;
        moveTarget = CurrentSector.GetComponent<Renderer>().bounds.center;
        currentTime = 0f;

        if (CurrentSector.SectorState != SectorState.Off) {
            Touchable = true;
            Debug.Log(this.name + " touchable");
            animator.SetTrigger("Pulse");
        }

        if (CurrentSector.SectorState == SectorState.Slide && lastSector.SectorState == SectorState.Slide) {
            trailRenderer.enabled = true;
        }
        else {
            trailRenderer.enabled = false;
            trailRenderer.Clear();
        }

        if (coroutine == null) {
            coroutine = MoveToNextSector(CurrentSector.transform);
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

//            if (percentage > .6f) Touchable = false;

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