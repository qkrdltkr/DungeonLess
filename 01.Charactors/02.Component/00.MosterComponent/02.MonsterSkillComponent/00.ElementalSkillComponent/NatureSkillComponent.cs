using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSkillComponent : DefaultSkillComponent {

    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
        _playerController = OperationData.Player.GetComponent<PlayerController>();
        LearnNatureSkill(_monsterStats, _monster);
    }
    public override void LearnNatureSkill(Character.Stats monsterStats, Transform monster) {
        if (this.ActiveSkills == null) this.ActiveSkills = new int[3];
        if (this.AcquiredSkills == null) this.AcquiredSkills = new List<int>();
        ////랜덤으로 자연 속성 스킬 습득(패시브면 1번만 쓰고 사라짐 액티브면 저장)

        int randomSkillnum = Random.Range(0, 2);
        switch (randomSkillnum) {
            case 0: // default Skills
                LearnDefaultSkill(monsterStats, monster);
                break;
            case 1:
                switch (Random.Range(SkillKind.kNaturalTmp1, SkillKind.kNaturalTmp1 + 1)) {//추후 수정 
                    //NatureSkills - Active
                    case SkillKind.kNaturalTmp1:
                        this.ActiveSkills[SkillCount] = SkillKind.kNaturalTmp1; this.SkillCount++;
                        AcquiredSkills.Add(SkillKind.kNaturalTmp1);
                        Debug.Log("자연속성 스킬을 배웠습니다"); break;
                }
                break;
        }
        CheckOverlap(SkillKind.kNaturalTmp1, SkillKind.kNaturalTmp1); //추후 수정
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

            //Nature Active
            case SkillKind.kNaturalTmp1: break;

        }
        return true;
    }
}
