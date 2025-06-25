using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    public List<SkillInfo> skills = new List<SkillInfo>();
    public CoolSystem[] cool;

    Player_Move player;

    public void Start()
    {
        player = Player_Move.instance;
    }

    public void Update()
    {
        if (!player.start)
        {
            return;
        }
        
        if (player.blockedStates.Contains(player.currentHash))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {// 좌클릭(일반공격)
            player.NormalAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {// 우클릭(방어)
            player.DefendSkill(1);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            player.DefendSkill(2);
        }
        else
        {
            player.DefendSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {// E스킬(QuickKnife)
            if (cool[1].isCooldown)
            {
                return;
            }

            cool[1].StartCool(skills[1].maxCooldownTime);
            player.ESkill();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {// Q스킬(CircleKnife)
            if (cool[2].isCooldown)
            {
                return;
            }

            cool[2].StartCool(skills[2].maxCooldownTime);
            player.QSkill();
        }
    }
}
