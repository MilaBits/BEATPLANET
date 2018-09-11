using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeUI : MonoBehaviour {
    private Node node;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI UnitText;

    private void Start() {
        node = GetComponentInParent<Node>();
    }

    private void Update() {
        if (node.owner) NameText.text = node.owner.Name;
        else NameText.text = "Neutral";
        UnitText.text = node.UnitCount.ToString();
    }
}