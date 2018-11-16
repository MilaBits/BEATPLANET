using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TouchDetect : MonoBehaviour {
    [BoxGroup("Detection")] public Camera Camera;
    [BoxGroup("Detection")] public LayerMask TouchLayer;

    public AudioSource Tap;
    public AudioSource Slide;

    public float TapCooldown = .1f;
    [BoxGroup("Tap", false)] private float lastTapTime = 0f;
    public TransformUnityEvent Tapped;

    public TransformUnityEvent Slided;

    [BoxGroup("Debug")] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowPaging = false)]
    public List<string> Taps = new List<string>();

    [BoxGroup("Debug")] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowPaging = false)]
    public List<string> slides = new List<string>();

    [System.Serializable]
    public class TransformUnityEvent : UnityEvent<Transform> {
    }

    Transform GetObjectFromTouch(Touch touch) {
        RaycastHit hit;

        Ray ray = Camera.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y));
        Physics.Raycast(ray, out hit, TouchLayer);

        return hit.transform;
    }

    void FixedUpdate() {
        if (Input.touches.Length < 1) {
            return;
        }

        GetTap();
    }

    private void GetTap() {
        int fingerCount = 0;
        Touch lastTouch = Input.touches.First();
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                fingerCount++;
                lastTouch = touch;
            }
        }

        float timeSinceLastTap = Time.time - lastTapTime;
        if (fingerCount >= 1 && timeSinceLastTap > TapCooldown) {
            Transform tapped = GetObjectFromTouch(lastTouch);
            foreach (Touch touch in Input.touches) {
                if (tapped == null) {
                    tapped = GetObjectFromTouch(touch);
                    continue;
                }

                break;
            }

            if (tapped) {
                Debug.Log("Tapped: " + tapped.name);
                if (tapped.GetComponent<Planet>() != null) {
                    Planet planet = tapped.GetComponent<Planet>();
                    if (planet.CurrentSector.SectorState != SectorState.Off) {
                        planet.Tap();
                        Tap.Play();
                    }
                }
                Tapped.Invoke(tapped);
                Taps.Add(tapped.name);
                lastTapTime = Time.time;
            }
        }
    }

    private void GetSlide() {
        int fingerCount = 0;
        Touch lastTouch = Input.touches.First();
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Moved && touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Ended) {
                fingerCount++;
                lastTouch = touch;
            }
        }

        if (fingerCount >= 1) {
            Transform slide = GetObjectFromTouch(lastTouch);
            foreach (Touch touch in Input.touches) {
                if (slide == null) {
                    slide = GetObjectFromTouch(touch);
                    continue;
                }

                break;
            }

            if (slide) {
                Slided.Invoke(slide);
                slides.Add(slide.name);
            }
        }
    }
}