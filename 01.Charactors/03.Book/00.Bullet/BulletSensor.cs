using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSensor : MonoBehaviour {
    public Transform Target { get; set; }

    private void OnTriggerEnter(Collider target) {
        if (!target.CompareTag("Monster")) return;
        Target = target.transform;
    }

    private void OnTriggerExit(Collider target) {
        if (!target.CompareTag("Monster")) return;
        Target = null;
    }

    private void OnDisable() {
        Target = null;
    }
}
