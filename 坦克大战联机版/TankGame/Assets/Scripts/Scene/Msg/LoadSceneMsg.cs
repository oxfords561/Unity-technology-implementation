using System;
using System.Collections.Generic;


public class LoadSceneMsg
{
    public int SceneBuildIndex;
    public string SceneName;
    public Action OnSceneLoaded;

    public LoadSceneMsg()
    {
        this.SceneBuildIndex = -1;
        this.SceneName = null;
        this.OnSceneLoaded = null;
    }

    public LoadSceneMsg(string name, Action loaded)
    {
        this.SceneBuildIndex = -1;
        this.SceneName = name;
        this.OnSceneLoaded = loaded;
    }

    public LoadSceneMsg(int index, Action loaded)
    {
        this.SceneBuildIndex = index;
        this.SceneName = null;
        this.OnSceneLoaded = loaded;
    }

}
