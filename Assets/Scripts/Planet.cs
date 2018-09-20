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

    public void NextSector(Vector3 target) {
        start = transform.position;
        moveTarget = target;
        currentTime = 0f;

        if (!slide) stepTime = TapTime;
        if (slide) stepTime = SlideTime;

        if (coroutine == null) {
            coroutine = MoveToNextSector();
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

    private void OnCollisionStay(Collision other) {
        Debug.Log("stay");
        if (other.gameObject.layer == SectorMask &&
            other.gameObject.GetComponent<Sector>().SectorState == SectorState.Slide) {
            slide = true;
        }
        else {
            slide = false;
        }
    }

    IEnumerator MoveToNextSector() {
        while (running) {
            percentage = currentTime / stepTime;
            transform.position = Vector3.Lerp(start, moveTarget, percentage);

            if (percentage > .95f) transform.position = moveTarget;
            currentTime += Time.deltaTime;
            yield return 0;
        }
    }

//    IEnumerator MoveToNextSector(Vector3 target) {
//        
//        while (Vector3.Distance(transform.position, target) > 0.01f) {
//            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
//            yield return null;
//        }
//    }
}