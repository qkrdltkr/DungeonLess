using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : IUpdateableObject {
    private Transform _player;

    protected override void Initialize() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnUpdate(float dt) {
        this.transform.rotation = Quaternion.Euler(0f, 0f, MainCamController.RotateDegree);
        this.transform.position = new Vector3(_player.position.x, _player.position.y - 0.25f,ObjectSortLevel.kWeapon);
    }
}
