using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.PROMPT_MSG);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PROMPT_MSG:
                PromptMsg msg = message as PromptMsg;
                promptMessage(msg.Text, msg.Color);
                break;
            default:
                break;
        }
    }

    private Text txt;
    private CanvasGroup cg;

    [SerializeField]
    [Range(0, 3)]
    private float showTime = 1f;

    private float timer = 0f;

    void Start()
    {
        txt = transform.Find("Text").GetComponent<Text>();
        cg = transform.Find("Text").GetComponent<CanvasGroup>();

        cg.alpha = 0;
    }

    /// <summary>
    /// 提示消息
    /// </summary>
    private void promptMessage(string text, Color color)
    {
        txt.text = text;
        txt.color = color;
        cg.alpha = 0;
        timer = 0;
        //做动画显示
        StartCoroutine(promptAnim());
    }

    /// <summary>
    /// 用来显示动画
    /// </summary>
    /// <returns></returns>
    IEnumerator promptAnim()
    {
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        while (timer < showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
    }

}
