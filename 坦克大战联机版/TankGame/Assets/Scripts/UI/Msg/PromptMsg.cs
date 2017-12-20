using System;
using System.Collections.Generic;
using UnityEngine;

public class PromptMsg
{
    public string Text;
    public Color Color;

    public PromptMsg()
    {

    }

    public PromptMsg(string text, Color color)
    {
        this.Text = text;
        this.Color = color;
    }

    /// <summary>
    /// 避免了频繁new对象
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    public void Change(string text, Color color)
    {
        this.Text = text;
        this.Color = color;
    }
}
