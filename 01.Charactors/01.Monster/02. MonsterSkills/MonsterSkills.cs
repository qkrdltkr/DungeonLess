using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkills : IUpdateableObject{
    public int Kind { get; set; }
    protected override void Initialize() { }
    protected Character _character;
    protected PlayerController _playerController;
    protected Transform _player;
    protected Transform _monster;
    protected Transform _target;
    protected Transform _hitBox;
    protected Condition _condition;
    protected float _delayTime = 0;
    protected float dis = 0;
}
