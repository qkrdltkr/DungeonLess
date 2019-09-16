using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessSkillComponent : DefaultSkillComponent {


    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
        _playerController = OperationData.Player.GetComponent<PlayerController>();
        LearnDarknessSkill(_monsterStats, _monster);
    }
    public override void LearnDarknessSkill(Character.Stats monsterStats, Transform monster) {
        if (this.ActiveSkills == null) this.ActiveSkills = new int[3];
        if (this.AcquiredSkills == null) this.AcquiredSkills = new List<int>();
        ////랜덤으로 암속성 스킬 습득(패시브면 1번만 쓰고 사라짐 액티브면 저장)

        int randomSkillnum = Random.Range(0, 2);
        switch (randomSkillnum) {
            case 0: // default Skills
                LearnDefaultSkill(monsterStats, monster);
                break;
            case 1:
                switch (Random.Range(SkillKind.kDarkNessTmp1, SkillKind.kDarkNessTmp1 + 1))//추후 수정
                {
                    //DarknessSkills - Active
                    case SkillKind.kDarkNessTmp1:
                        this.ActiveSkills[SkillCount] = SkillKind.kDarkNessTmp1; this.SkillCount++;
                        AcquiredSkills.Add(SkillKind.kDarkNessTmp1);
                        Debug.Log("어둠속성 스킬을 배웠습니다"); break;
                }
                break;
        }
        CheckOverlap(SkillKind.kDarkNessTmp1, SkillKind.kDarkNessTmp1); //추후 수정
    }


    public override bool UseSkill()
    {
        //가지고있는 액티브 스킬중 랜덤으로 발사
        if (SkillCount == 0) return false; // 스킬이 없으면 리턴
        int randomIdx = Random.Range(0, SkillCount);

        switch (ActiveSkills[randomIdx])
        {
            //Default Active
            case SkillKind.kHeadHunter: HeadHunter(_monsterStats, _playerController); break;
            case SkillKind.kSmash: Smash(_monsterStats, _player); break;
            case SkillKind.kTwiceAttack: TwiceAttack(_monsterStats); break;

            //Darkness Active
            case SkillKind.kDarkNessTmp1: break;

        }
        return true;
    }

}
