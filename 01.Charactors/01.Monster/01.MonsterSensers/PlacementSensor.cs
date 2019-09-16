using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSensor : MonoBehaviour {
    public bool CanPlacement { get; set; }
    public string[] _Tags;

    // Stay -> Enter로 바꿀 예정
    private void OnTriggerStay(Collider target) {
        for(int i = 0; i < _Tags.Length; ++i)
            if (target.CompareTag(_Tags[i])) {
                CanPlacement = true;
                return;
            }
    }
    private void OnTriggerExit(Collider target) {
        for (int i = 0; i < _Tags.Length; ++i)
            if (target.CompareTag(_Tags[i])) {
                CanPlacement = false;
                return;
            }
    }
}
