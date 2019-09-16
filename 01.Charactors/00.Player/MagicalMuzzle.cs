using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalMuzzle : IUpdateableObject {
    public Transform[] WepaonAxis;
    private MuzzleEvent _muzzle;

    protected override void Initialize() {
    }

    public void AddEvent(MuzzleEvent e) {
        _muzzle += e;
    }

    public void DeleteEvent(MuzzleEvent e) {
        _muzzle -= e;
    }

    public void UseSpell(Character.Stats stats) {
        _muzzle(WepaonAxis, stats, this.transform);
    }
}
