using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 存储所有的UI事件码
/// </summary>
public class UIEvent
{
    public const int LOGIN_PANEL_ACTIVE = 0;//设置开始面板的显示
    public const int REGIST_PANEL_ACTIVE = 1;//设置注册面板的显示
    public const int READY_PANEL_ACTIVE = 2;//设置准备面板显示
    //....

    public const int PROMPT_MSG = int.MaxValue;

    public const int READY_COUNT_SHOW = 3;//准备面板 人数显示
    public const int HOUSE_OWNER = 4; //房主 按钮 显示

    public const int GAMEOVER_PANEL_ACTIVE = 5;//游戏结束面板的显示
    public const int GAMEVICTORY_PANEL_ACTIVE = 6; // 游戏胜利面板显示
}
