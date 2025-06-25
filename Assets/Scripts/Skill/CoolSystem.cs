using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CoolSystem : MonoBehaviour
{
    public float currentCooldownTime;
    public bool isCooldown;
    public Image imageCooldownTime;
    public TextMeshProUGUI textCooldownTime;

    private void Awake()
    {
        SetCooldownIs(false);
    }
    void Start()
    {
        isCooldown = false;
    }

    public void StartCool(float Cool)
    {
        StartCoroutine(nameof(OnCooldownTime), Cool);
    }

    private IEnumerator OnCooldownTime(float maxCooldownTime)
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
}
