using UnityEngine;
using UnityEngine.UI;

public class SkillSubtitleDisplay : MonoBehaviour
{
    private static SkillSubtitleDisplay instance;

    public static SkillSubtitleDisplay Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillSubtitleDisplay>();
                if (instance == null)
                {
                    Debug.LogError("The SkillSubtitleDisplay is missing in the scene!");
                }
            }
            return instance;
        }
    }

    public float displayTime = 3f;                // 技能名称显示时间
    public float moveSpeed = 10f;                 // 字幕上移的速度
    public float fadeSpeed = 2f;                  // 字幕淡出的速度
    public float maxPositionY = 50f;              // 字幕上升到的最大位置
    public GameObject skillEffect;                // 技能执行特效
    public Text skillNameText;                    // 用于显示技能名称的UI Text组件

    private bool isDisplayingName;                // 是否正在显示技能名称
    private float timer;                          // 计时器

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isDisplayingName)
        {
            timer += Time.deltaTime;

            if (timer >= displayTime)
            {
                // 时间到，停止显示技能名称
                HideSkillName();
                ExecuteSkill();
            }
            else
            {
                // 上移字幕，直到达到最大位置停止移动
                if (skillNameText.transform.position.y <= maxPositionY)
                {
                    Vector3 newPosition = skillNameText.transform.position + moveSpeed * Vector3.up * Time.deltaTime;
                    skillNameText.transform.position = newPosition;
                }

                // 淡出效果
                float fadeOutAlpha = Mathf.Lerp(1f, 0f, (timer - displayTime + fadeSpeed) / fadeSpeed);
                float alpha = Mathf.Lerp(1f, fadeOutAlpha, Mathf.Clamp01(timer / displayTime));
                skillNameText.color = new Color(skillNameText.color.r, skillNameText.color.g, skillNameText.color.b, alpha);
            }
        }
    }

    public void ShowSkillSubtitle(string skillName, Vector3 position)
    {
        if (!isDisplayingName)
        {
            skillNameText.text = skillName;
            skillNameText.transform.position = position;

            isDisplayingName = true;
            timer = 0f;
        }
    }

    void HideSkillName()
    {
        if (isDisplayingName)
        {
            skillNameText.text = "";
            skillNameText.color = new Color(skillNameText.color.r, skillNameText.color.g, skillNameText.color.b, 1f);

            isDisplayingName = false;
            timer = 0f;
        }
    }

    void ExecuteSkill()
    {
        Instantiate(skillEffect, transform.position, transform.rotation);
        Debug.Log(skillNameText.text + " skill activated!");
    }
}
