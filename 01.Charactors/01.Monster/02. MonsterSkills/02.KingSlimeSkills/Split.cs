using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonsterSkills {
    public GameObject _Kingslime;
    private bool _isSplit;
    public void Init(Transform monster, Condition condition) {
        _condition = condition;
        _monster = monster;
        _hitBox = _monster.Find("HitBox");
    }
    protected override void Initialize() { }
    public void SplitKingSlime() {
        _condition.SetCondition(Condition.kAttack, (int)KingSlimeSkillKInds.SPLIT);
        _hitBox.GetComponent<Rigidbody>().isKinematic = true;
        //크기 줄이기
        _monster.transform.localScale = new Vector3(_monster.localScale.x * 0.65f,
                                                    _monster.localScale.y * 0.65f,
                                                    _monster.localScale.z * 0.65f);
        //체력게이지 없애기
        _hitBox.GetChild(0).transform.localScale = Vector3.zero;
        _hitBox.GetChild(1).transform.localScale = Vector3.zero;

        _isSplit = true;
    }
    public override void OnUpdate(float dt) {
        if (!_isSplit) return; _delayTime += dt;
        if (_delayTime > 1.8f) {

            //킹슬라임 생성
            Instantiate(_Kingslime, new Vector3(_monster.position.x - 6,
                                                _monster.position.y,
                                                _monster.position.z),
                                                Quaternion.identity);
            Instantiate(_Kingslime, new Vector3(_monster.position.x + 6,
                                                _monster.position.y,
                                                _monster.position.z),
                                                Quaternion.identity);
            //본체없애기
            _monster.gameObject.SetActive(false);
            _hitBox.tag = "Untagged";
        }
    }
}
