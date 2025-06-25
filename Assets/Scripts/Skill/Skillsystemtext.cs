using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;

public class Skillsystemtext : MonoBehaviour
{
    /*
    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    [SerializeField]
    private SkilText[] skills;

    private List<RaycastResult> raycastResults;
    private PointerEventData pointerEventData;

    private void Awake()
    {
        GameObject.Find("Player").GetComponent<Player_Move>();
        raycastResults = new List<RaycastResult>();
        pointerEventData = new PointerEventData(null);
    }

    private void Update()
    {
        //if (!Input.anyKeyDown) return;

        if (Input.GetMouseButtonDown(1))
        {
            skills[3].defendkeydown = true;
            skills[3].UseSkill();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            skills[3].defendkeydown = false;
            skills[3].UseSkill();
        }

        if (Input.GetMouseButtonDown(0))
        {
            skills[0].UseSkill();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skills[1].UseSkill();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skills[2].UseSkill();
        }
        /*
        // 숫자키 (NumPad 포함) 스킬 제어
        if (int.TryParse(Input.inputString, out int key) && (key >= 1 && key <= skills.Length))
        {
            skills[key - 1].UseSkill();
        }
        */

    /*
    // 마우스로 해당 스킬 아이콘을 클릭
    if (Input.GetMouseButton(0))
    {
        raycastResults.Clear();

        pointerEventData.position = Input.mousePosition;
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            if (raycastResults[0].gameObject.TryGetComponent<Skill>(out var skill))
            {
                skill.UseSkill();
            }
        }
    }
    
    }
    */
}
