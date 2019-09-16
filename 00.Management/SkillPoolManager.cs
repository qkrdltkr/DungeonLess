using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoolManager : Singleton<SkillPoolManager> {
    public static SkillPoolManager Instance {
        get { return (SkillPoolManager)_Instance; }
        set { _Instance = value; }
    }

    public GameObject[] _SkillPrefabs;
    private SkillKind _skillkind;

    private List<GameObject> skills = new List<GameObject>();
    private GameObject _child;


    private GameObject _poolChild;
    private void Start() {
        Transform Parent = this.transform;
        for (int i = 0; i < SkillKind.maxSkill; i++) {
            _child = new GameObject();
            SetChild(i);
            switch (i) {
                //Default Skills
                case SkillKind.kHaste: _child.name = "Haste"; break;
                case SkillKind.kRapid: _child.name = "Rapid"; break;
                case SkillKind.kSharp: _child.name = "Sharp"; break;
                case SkillKind.kHealty: _child.name = "Healty"; break;
                case SkillKind.kVolant: _child.name = "Volant"; break;
                case SkillKind.kSonic: _child.name = "Sonic"; break;
                case SkillKind.kSuperArmor: _child.name = "SuperArmor"; break;
                case SkillKind.kGiant: _child.name = "Giant"; break;
                case SkillKind.kHeadHunter: _child.name = "HeadHunter"; break;
                case SkillKind.kSmash: _child.name = "Smash"; break;
                case SkillKind.kTwiceAttack: _child.name = "TwiceAttack"; break;
                //Flame Skills
                case SkillKind.kLava: _child.name = "LavaPool"; break;
                case SkillKind.kFlameColak: _child.name = "FlameCloakPool"; break;
                case SkillKind.kFlameTmp1: _child.name = "kFlameTmp1"; break;
                case SkillKind.kFlameTmp2: _child.name = "kFlameTmp2"; break;
                case SkillKind.kFlameTmp3: _child.name = "kFlameTmp3"; break;
                case SkillKind.kFlameTmp4: _child.name = "kFlameTmp4"; break;
                case SkillKind.kFlameTmp5: _child.name = "kFlameTmp5"; break;
                case SkillKind.kFlameTmp6: _child.name = "kFlameTmp6"; break;
                case SkillKind.kFlameTmp7: _child.name = "kFlameTmp7"; break;
                case SkillKind.kFlameTmp8: _child.name = "kFlameTmp8"; break;
                //Water Skills
                case SkillKind.kSlow: _child.name = "kSlow"; break;
                case SkillKind.kWaterTmp1: _child.name = "kWaterTmp1"; break;
                case SkillKind.kWaterTmp2: _child.name = "kWaterTmp2"; break;
                case SkillKind.kWaterTmp3: _child.name = "kWaterTmp3"; break;
                case SkillKind.kWaterTmp4: _child.name = "kWaterTmp4"; break;
                case SkillKind.kWaterTmp5: _child.name = "kWaterTmp5"; break;
                case SkillKind.kWaterTmp6: _child.name = "kWaterTmp6"; break;
                case SkillKind.kWaterTmp7: _child.name = "kWaterTmp7"; break;
                case SkillKind.kWaterTmp8: _child.name = "kWaterTmp8"; break;
                case SkillKind.kWaterTmp9: _child.name = "kWaterTmp9"; break;
                //Natural Skills
                case SkillKind.kNaturalTmp1: _child.name = "kNaturalTmp1"; break;
                case SkillKind.kNaturalTmp2: _child.name = "kNaturalTmp2"; break;
                case SkillKind.kNaturalTmp3: _child.name = "kNaturalTmp3"; break;
                case SkillKind.kNaturalTmp4: _child.name = "kNaturalTmp4"; break;
                case SkillKind.kNaturalTmp5: _child.name = "kNaturalTmp5"; break;
                case SkillKind.kNaturalTmp6: _child.name = "kNaturalTmp6"; break;
                case SkillKind.kNaturalTmp7: _child.name = "kNaturalTmp7"; break;
                case SkillKind.kNaturalTmp8: _child.name = "kNaturalTmp8"; break;
                case SkillKind.kNaturalTmp9: _child.name = "kNaturalTmp9"; break;
                case SkillKind.kNaturalTmp10: _child.name = "kNaturalTmp10"; break;
                //Light Skills
                case SkillKind.kLightTmp1: _child.name = "kLightTmp1"; break;
                case SkillKind.kLightTmp2: _child.name = "kLightTmp2"; break;
                case SkillKind.kLightTmp3: _child.name = "kLightTmp3"; break;
                case SkillKind.kLightTmp4: _child.name = "kLightTmp4"; break;
                case SkillKind.kLightTmp5: _child.name = "kLightTmp5"; break;
                case SkillKind.kLightTmp6: _child.name = "kLightTmp6"; break;
                case SkillKind.kLightTmp7: _child.name = "kLightTmp7"; break;
                case SkillKind.kLightTmp8: _child.name = "kLightTmp8"; break;
                case SkillKind.kLightTmp9: _child.name = "kLightTmp9"; break;
                case SkillKind.kLightTmp10: _child.name = "kLightTmp10"; break;
                //DarkNess Skills
                case SkillKind.kDarkNessTmp1: _child.name = "kDarkNessTmp1"; break;
                case SkillKind.kDarkNessTmp2: _child.name = "kDarkNessTmp2"; break;
                case SkillKind.kDarkNessTmp3: _child.name = "kDarkNessTmp3"; break;
                case SkillKind.kDarkNessTmp4: _child.name = "kDarkNessTmp4"; break;
                case SkillKind.kDarkNessTmp5: _child.name = "kDarkNessTmp5"; break;
                case SkillKind.kDarkNessTmp6: _child.name = "kDarkNessTmp6"; break;
                case SkillKind.kDarkNessTmp7: _child.name = "kDarkNessTmp7"; break;
                case SkillKind.kDarkNessTmp8: _child.name = "kDarkNessTmp8"; break;
                case SkillKind.kDarkNessTmp9: _child.name = "kDarkNessTmp9"; break;
                case SkillKind.kDarkNessTmp10: _child.name = "kDarkNessTmp10"; break;
            }
        }
        int skillcount = 0;
        for (int i = 0; i < 6; ++i)  {
            for (int j = 0; j < Parent.GetChild(i).childCount; j++) {
                FullPool(skillcount, Parent.GetChild(i).GetChild(j));
                skillcount++;
            }
        }
    }

    private void SetChild(int i)
    {
        if (i < SkillKind.kLava) _child.transform.parent = this.transform.GetChild(0);
        else if (i < SkillKind.kSlow) _child.transform.parent = this.transform.GetChild(1);
        else if (i < SkillKind.kNaturalTmp1) _child.transform.parent = this.transform.GetChild(2);
        else if (i < SkillKind.kLightTmp1) _child.transform.parent = this.transform.GetChild(3);
        else if (i < SkillKind.kDarkNessTmp1) _child.transform.parent = this.transform.GetChild(4);
        else if (i <= SkillKind.maxSkill) _child.transform.parent = this.transform.GetChild(5);
    }

    private void FullPool(int kind, Transform parent) {
        if (kind == SkillKind.kLava || kind == SkillKind.kFlameColak || kind == SkillKind.kSlow)  {
            for (int i = 0; i < 10; i++) {
                _poolChild = Instantiate(_SkillPrefabs[kind]);
                _poolChild.SetActive(true);
                _poolChild.transform.parent = parent;
            }
        }
    }
    public void RechargeSkill(GameObject skill, Transform kind) {
        skill.transform.parent = kind;
        skills.Add(skill);
        skill.SetActive(false);
    }

    public void SetSkill(GameObject skill, Transform kind) {
        skill.transform.parent = kind;
        skills.Add(skill);
    }
    public GameObject PopSkill(int kind) {
        int skillindex = 0;
        for (int j = 0; j < skills.Count; j++)
        {
            if (skills[j].GetComponent<MonsterSkills>().Kind != kind) skillindex++;
            else j = skills.Count;
        }
        GameObject skill;

        if (skillindex == skills.Count) skill = SupplySkill(kind);
        else skill = skills[skillindex];

        skills.RemoveAt(skillindex);
        return skill;
    }
    private GameObject SupplySkill(int kind) {
        GameObject skill = Instantiate(_SkillPrefabs[kind], this.transform.position, Quaternion.identity, this.transform);
        skills.Add(skill);
        return skill;
    }
}
