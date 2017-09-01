using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityTools {

    public static Vector3 ParseVector3(string data)
    {
        string[] strs = data.Split(',');
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);

        return new Vector3(x, y, z);
    }
}
