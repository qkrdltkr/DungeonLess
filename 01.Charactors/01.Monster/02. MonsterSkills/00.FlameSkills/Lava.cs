using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonsterSkills {
    protected override void Initialize() {
        this.Kind = SkillKind.kLava;
        SkillPoolManager.Instance.SetSkill(this.gameObject, this.transform.parent);
        this.gameObject.SetActive(false);
    }
    public void Burning(Transform _monster) {
        this.gameObject.SetActive(true);
        this.transform.position = _monster.transform.position;
       
        StartCoroutine(DurationBurning());
    }
    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            _playerController = col.GetComponent<PlayerController>();
            _playerController.Damaged(Vector3.zero, 5, StatusEffectKind.kBurning);
        }
        else if (col.CompareTag("Hero")) {

        }
    }
    private IEnumerator DurationBurning() {
        yield return YieldInstructionCache.WaitForSeconds(10.0f);
        SkillPoolManager.Instance.RechargeSkill(this.gameObject, this.transform.parent);
        StopCoroutine(DurationBurning());
    }
} 
