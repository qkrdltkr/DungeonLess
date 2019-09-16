using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : IUpdateableObject {
 
    protected Character.Stats _targetStats;
    protected Transform _targetPos;
    public float _durationTime = 0;
    public int Kind { get; set; }
    public virtual bool isFinished() { return false; }
    protected override void Initialize() { }
    public virtual void BurningTarget(Transform target) { }
   
}
