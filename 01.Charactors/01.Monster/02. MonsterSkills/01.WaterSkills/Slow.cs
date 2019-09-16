using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonsterSkills  {
    protected override void Initialize() {
        this.Kind = SkillKind.kSlow;
        SkillPoolManager.Instance.SetSkill(this.gameObject, this.transform.parent);
    }
    public void Slowing(Transform _target) {
        this.transform.position = _target.transform.position;
        this.gameObject.SetActive(true);

        StartCoroutine(DurationSlowing());
    }
    private void OnTriggerEnter(Collider col) {
        if (!col.CompareTag("Player")) return;
        _playerController = col.GetComponent<PlayerController>();
        StartCoroutine(SlowPlayer(_playerController));
    }
    private IEnumerator DurationSlowing()
    {
        yield return YieldInstructionCache.WaitForSeconds(5.0f);
        SkillPoolManager.Instance.RechargeSkill(this.gameObject, this.transform.parent);
        StopCoroutine(DurationSlowing());
    }
    private IEnumerator SlowPlayer(PlayerController _player)
    {
        int i = 0;
        while (true) {
          //  플레이어를 슬로우시킨다
            yield return YieldInstructionCache.WaitForSeconds(1.0f);
            i++;
            if (i > 3) StopCoroutine(SlowPlayer(_player));
        }
    }
}
