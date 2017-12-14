using UnityEngine;

public class SnakeCell
{

    private Vector2 coord;
    private string spriteName;

    public Vector2 Coord { get { return coord; } set { coord = value; } }
    public string SpriteName { get { return spriteName; } set { spriteName = value; } }
}
