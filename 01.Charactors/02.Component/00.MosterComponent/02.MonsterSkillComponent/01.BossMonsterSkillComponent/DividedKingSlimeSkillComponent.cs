using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedKingSlimeSkillComponent : MonsterSkillComponent {
    private bool _isHalfScale;
    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
        _hitBox.GetComponent<Jump>().Init(_monster, _condition);
    }
    public override void LearnKingSlimeSkill(Character.Stats monsterStats, Transform monster) {
        if (this.ActiveSkills == null) this.ActiveSkills = new int[4] { -1, -1, -1, -1 };
        if (this.AcquiredSkills == null) this.AcquiredSkills = new List<int>();

        for (int i = 0; i < 4; i++) {
            this.ActiveSkills[i] = i; this.SkillCount++;
            AcquiredSkills.Add(i);
        }
    }
    public override void Clear() {
        isCanUseSkill = false;
        _target = null;
    }
    public override bool UseSkill() {
        if (!isCanUseSkill) return false;
        //가지고있는 액티브 스킬중 랜덤으로 발사
        if (SkillCount == 0) return false; // 스킬이 없으면 리턴
        if (_condition.currentCondition.Equals(Condition.kAttack)) return false; //이미 공격중이면 리턴
        int randomIdx = Random.Range(0, SkillCount - 1);

        switch (ActiveSkills[randomIdx]) {
            case BossMonsterSkillKind.kKingSlimeJump: /*JumpAttack()*/; break;
            case BossMonsterSkillKind.kKingSlimeSpit: Spit(); break;
            case BossMonsterSkillKind.kKingSlimeRadiationSpit: RadialSpit(); break;
        }
        return true;
    }
    public override void JumpAttack() {
        _hitBox.GetComponent<Jump>().JumpAttack(_hitBox);
    }
    public override void Spit() {
        Debug.Log("슬라임 퉤퉤!!");
        GameObject bullet = MonsterBulletPoolManager.Instance.PopBullet(MonsterBulletKind.kMucus);
        SpitBullet spit = bullet.GetComponent<SpitBullet>();
        spit.Fire(_monster, _target, _condition);
    }
    public override void RadialSpit() {
        Debug.Log("슬라임 방사형 퉤퉤!!");
        GameObject bullet = MonsterBulletPoolManager.Instance.PopBullet(MonsterBulletKind.kRadialMucus);
        RadialSpitBullet radialSpit = bullet.GetComponent<RadialSpitBullet>();
        radialSpit.Fire(_monster, _target, _condition);
    }
    public override void DoWork(Transform tr) {
        if (!tr) {
            isCanUseSkill = false;
            return;
        }
        isCanUseSkill = true;
    }

}
