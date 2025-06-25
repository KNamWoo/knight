using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using InventorySampleScene;

public class SkilText : MonoBehaviour
{
    /*

    private float currentCooldownTime;
    private bool isCooldown;

    Player_Move player;

    public bool defendkeydown;

    private void Awake()
    {
        SetCooldownIs(false);
    }

    private void Start()
    {
        player = Player_Move.instance;
    }

    public void UseSkill()
    {
        if (isCooldown == true)
        {
            textSkillData.text = $"[{skillName}] Cooldown Time : {currentCooldownTime:F1}";
            return;
        }

        if (player.blockedStates.Contains(player.currentHash))
        {
            return;
        }

        if (skillName == "Defend" && defendkeydown)
        {
            player.buttonUp = false;
            player.defending = true;
            player.animator.Play("Player_Defend");
            player.canPlay = false;
            Debug.Log("Down");
        }
        else if (skillName == "Defend" && !defendkeydown)
        {
            player.buttonUp = true;
            Debug.Log("Up");
        }
        else if (player.buttonUp == true && player.defending == false)
        {
            player.canPlay = true;
        }

        textSkillData.text = $"Use Skill : {skillName}";

        if (skillName == "Normal")
        {
            player.buttonUp = true;
            Debug.Log("attack");
            player.animator.Play("Player_Attack1");
        }
        else if (skillName == "QuickKnife")
        {
            player.buttonUp = true;
            player.animator.Play("Player_Attack2");
        }
        else if (skillName == "CircleKnife")
        {
            player.buttonUp = true;
            player.animator.Play("Player_Attack3");
        }

        StartCoroutine(nameof(OnCooldownTime), maxCooldownTime);
    }
    public void StartCooldown()
    {
        StartCoroutine(nameof(OnCooldownTime), maxCooldownTime);
    }

    public IEnumerator OnCooldownTime(float maxCooldownTime)
    {
        currentCooldownTime = maxCooldownTime;

        SetCooldownIs(true);

        while (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
            // 쿨다운 이미지 채움 설정
            imageCooldownTime.fillAmount = currentCooldownTime / maxCooldownTime;
            // 쿨다운 시간 표시
            textCooldownTime.text = currentCooldownTime.ToString("F1");

            yield return null;
        }

        SetCooldownIs(false);
    }

    private void SetCooldownIs(bool boolean)
    {
        isCooldown = boolean;
        textCooldownTime.enabled = boolean;
        imageCooldownTime.enabled = boolean;
    }
    */
}
