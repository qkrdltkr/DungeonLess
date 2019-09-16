using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameColak : MonsterSkills {
    
    protected override void Initialize() {
        this.Kind = SkillKind.kFlameColak;
        SkillPoolManager.Instance.SetSkill(this.gameObject, this.transform.parent);
        this.gameObject.SetActive(false);
    }

    public override void OnUpdate(float dt) {
        if (_player == null) return;
        dis = Vector3.Distance(_player.position, this.transform.position);
        if(dis < 2f) {
            _delayTime += dt;
            if (_delayTime > 2f) {
                _playerController.Damaged(Vector3.zero, 5, StatusEffectKind.kNoStatusEffect);
            _delayTime = 0.0f;
            }
        }
    }
    public void Burning(Transform monster, Transform target) {
        _player = target;
        _playerController = _player.GetComponent<PlayerController>();
        this.transform.position = monster.transform.position;
        this.gameObject.SetActive(true);
        StartCoroutine(DurationBurning());
    }
    private IEnumerator DurationBurning() {
        yield return YieldInstructionCache.WaitForSeconds(5.0f);
        SkillPoolManager.Instance.RechargeSkill(this.gameObject, this.transform.parent);
        StopCoroutine(DurationBurning());
    }
}
