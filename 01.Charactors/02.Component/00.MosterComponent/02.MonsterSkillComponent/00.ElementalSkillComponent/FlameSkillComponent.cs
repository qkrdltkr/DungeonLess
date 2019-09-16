using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSkillComponent : DefaultSkillComponent {


    public override void SetTarget(Transform target) {
        _target = target;
    }
    public override void Initialize(Transform origin, Condition condition) {
        _monster = origin;
        _hitBox = _monster.Find("HitBox");
        _monsterStats = _hitBox.GetComponent<Monster>().GetStats();
        _condition = condition;
        _playerController = OperationData.Player.GetComponent<PlayerController>();
        LearnFlameSkill(_monsterStats, _monster);
    }
    public override void LearnFlameSkill(Character.Stats monsterStats, Transform monster) {
        if (this.ActiveSkills == null) this.ActiveSkills = new int[3] { -1,-1,-1 };
        if (this.AcquiredSkills == null) this.AcquiredSkills = new List<int>();
        ////랜덤으로 화염스킬 습득(패시브면 1번만 쓰고 사라짐 액티브면 저장)
        ///
        int randomSkillnum = Random.Range(0, 2);
        switch (randomSkillnum)
        {
            case 0: // default Skills
                LearnDefaultSkill(monsterStats, monster);
                break;
            case 1:
                switch(Random.Range(SkillKind.kLava, SkillKind.kFlameColak + 1)) {
                    //FlameSkills - Active
                    case SkillKind.kLava:
                        this.ActiveSkills[SkillCount] = SkillKind.kLava; this.SkillCount++;
                        AcquiredSkills.Add(SkillKind.kLava);
                        Debug.Log("라바를 배웠습니다"); break;
                    case SkillKind.kFlameColak:
                        this.ActiveSkills[SkillCount] = SkillKind.kFlameColak; this.SkillCount++;
                        AcquiredSkills.Add(SkillKind.kFlameColak);
                        Debug.Log("호염망토를 배웠습니다"); break;
                }
                break;
        }
        CheckOverlap(SkillKind.kLava, SkillKind.kFlameColak);
    }

    public override bool UseSkill() {
        //가지고있는 액티브 스킬중 랜덤으로 발사
        if (SkillCount == 0) return false; // 스킬이 없으면 리턴
        int randomIdx = Random.Range(0, SkillCount);

        switch (ActiveSkills[randomIdx]) {
            //Default Active
            case SkillKind.kHeadHunter:  HeadHunter(_monsterStats, _playerController);  break;
            case SkillKind.kSmash:       Smash(_monsterStats, _player);       break;
            case SkillKind.kTwiceAttack: TwiceAttack(_monsterStats); break;

            //Flame Active
            case SkillKind.kLava:         Lava();       break;           
            case SkillKind.kFlameColak:   FlameCloak(); break;
        }
        return true;
    }

    public override void Lava() {
        GameObject lavaObject = SkillPoolManager.Instance.PopSkill(SkillKind.kLava);
        Lava lava = lavaObject.GetComponent<Lava>();
        lava.Burning(_monster);
    }
    public override void FlameCloak() {
        GameObject _flameColakObject = SkillPoolManager.Instance.PopSkill(SkillKind.kFlameColak);
        FlameColak flameColak = _flameColakObject.GetComponent<FlameColak>();
        flameColak.Burning(_monster, _player);
    }
}
