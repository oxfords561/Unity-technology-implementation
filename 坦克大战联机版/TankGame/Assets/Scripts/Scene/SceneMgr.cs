using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance = null;

    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg msg = message as LoadSceneMsg;
                loadScene(msg);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 临时变量
    /// </summary>
    private Action OnSceneLoaded = null;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    private void loadScene(LoadSceneMsg msg)
    {
        if (msg.SceneBuildIndex != -1)
            SceneManager.LoadScene(msg.SceneBuildIndex);

        if (msg.SceneName != null)
            SceneManager.LoadScene(msg.SceneName);

        if (msg.OnSceneLoaded != null)
            OnSceneLoaded = msg.OnSceneLoaded;
    }

    /// <summary>
    /// 当场景加载完成的时候调用
    /// </summary>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (OnSceneLoaded != null)
            OnSceneLoaded();
    }

}

