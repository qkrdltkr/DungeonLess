using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonsterSkills {
    private bool _isJumpStart;
    private Transform _jumpTr;
    private float _originPos;
    private bool _isMonsterTranslated = true;
    public void Init(Transform monster, Condition condition) {
        _monster = monster;
        _condition = condition;
    }
    protected override void Initialize() { }
    public void JumpAttack(Transform target){
        _target = target;
        _originPos = _monster.position.y;
        _jumpTr = _monster.Find("JumpPos").transform;
        _monster.position = new Vector3(_monster.position.x,
            _jumpTr.position.y, _monster.position.z);
        _condition.SetCondition(Condition.kAttack, (int)KingSlimeSkillKInds.JUMP);
        _isJumpStart = true;
    }
    public override void OnUpdate(float dt) {
        if(_isJumpStart) {
            _delayTime += dt;
            if(_delayTime> 1.0f) { //점프하여 이동 
                if (_isMonsterTranslated) {
                    _isMonsterTranslated = false;
                    _monster.position = new Vector3(_target.position.x,
                    _monster.position.y, _target.position.z);
                }
            }
            if (_delayTime > 2.0f)  { //착지후
                _monster.position = new Vector3(_monster.position.x,
               _originPos, _monster.position.z);
                _condition.SetCondition(Condition.kIdle);
                _delayTime = 0;
                _isJumpStart = false;
                _isMonsterTranslated = true;
            }
        }
    }

}
