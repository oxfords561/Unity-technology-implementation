using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    [HideInInspector]
    public List<SnakeCell> snakeBody = new List<SnakeCell>();
    private List<SnakeCell> snakeBodyTemp = new List<SnakeCell>();
    private ChessBoard chessBoard;
    private GameController gameController;
    //贪吃蛇头部
    public Vector2 head;
    //贪吃蛇脖子
    private Vector2 neck;
    //贪吃蛇尾巴
    public Vector2 tail;
    //贪吃蛇运动的方向
    public Direction direction = Direction.None;
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    //事件委托
    //public delegate void CreateFoodHandler(GameController gameController);
    //public event CreateFoodHandler CreateFoodEvent;


    public void Init()
    {

        chessBoard = GetComponent<ChessBoard>();

        gameController = GetComponent<GameController>();

        InitData();

        InvokeRepeating("SnakeWalk", 0.5f, 0.3f);
    }

    public void InitData()
    {
        snakeBody.Clear();

        SnakeCell snakeCell = new SnakeCell();

        snakeCell.Coord = new Vector2(3, 7);
        snakeCell.SpriteName = "正方形";
        snakeBody.Add(snakeCell);

        SnakeCell snakeCell2 = new SnakeCell();
        snakeCell2.Coord = new Vector2(3, 6);
        snakeCell2.SpriteName = "圆形";
        snakeBody.Add(snakeCell2);

        SnakeCell snakeCell3 = new SnakeCell();
        snakeCell3.Coord = new Vector2(3, 5);
        snakeCell3.SpriteName = "圆形";
        snakeBody.Add(snakeCell3);

        chessBoard.SetSpriteImage(snakeBody);
    }

    public void AddSnakeBody(Vector2 coord, string spriteName = "圆形")
    {

        //if (CreateFoodEvent != null)
        //{
        //    CreateFoodEvent(gameController);
        //}

        SnakeCell snakeCell = new SnakeCell();
        snakeCell.Coord = coord;
        snakeCell.SpriteName = spriteName;
        snakeBody.Add(snakeCell);
    }

    private void SnakeWalk()
    {
        if (gameController.isLife)
        {
            //计算贪吃蛇运动方向
            if (direction == Direction.None)
                CalculateDirection();
            //贪吃蛇进行运动
            CalculateWalkLine();
            //进行运动
            chessBoard.SetSpriteImage(snakeBody);
        }
        
    }


    private void CalculateDirection()
    {
        //1、确定贪吃蛇的运动方向
        head = snakeBody[0].Coord;
        neck = snakeBody[1].Coord;
        Vector2 dir = head - neck;

        if (dir == new Vector2(1, 0))
        {
            direction = Direction.Down;
        }
        else if (dir == new Vector2(-1, 0))
        {
            direction = Direction.Up;
        }
        else if (dir == new Vector2(0, 1))
        {
            direction = Direction.Right;
        }
        else if (dir == new Vector2(0, -1))
        {
            direction = Direction.Left;
        }
    }

    private void CalculateWalkLine()
    {
        CopySnakeBody();

        if (direction == Direction.Up)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i].Coord = new Vector2(snakeBodyTemp[i - 1].Coord.x, snakeBodyTemp[i - 1].Coord.y);
            }
            snakeBody[0].Coord = new Vector2(snakeBody[0].Coord.x - 1, snakeBody[0].Coord.y);
        }
        else if (direction == Direction.Down)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i].Coord = new Vector2(snakeBodyTemp[i - 1].Coord.x, snakeBodyTemp[i - 1].Coord.y);
            }
            snakeBody[0].Coord = new Vector2(snakeBody[0].Coord.x + 1, snakeBody[0].Coord.y);
        }
        else if (direction == Direction.Left)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                snakeBody[i].Coord = new Vector2(snakeBodyTemp[i - 1].Coord.x, snakeBodyTemp[i - 1].Coord.y);
            }
            snakeBody[0].Coord = new Vector2(snakeBody[0].Coord.x, snakeBody[0].Coord.y - 1);
        }
        else if (direction == Direction.Right)
        {
            for (int i = 1; i < snakeBodyTemp.Count; i++)
            {
                snakeBody[i].Coord = new Vector2(snakeBodyTemp[i - 1].Coord.x, snakeBodyTemp[i - 1].Coord.y);
            }

            snakeBody[0].Coord = new Vector2(snakeBodyTemp[0].Coord.x, snakeBodyTemp[0].Coord.y + 1);

        }

        head = snakeBody[0].Coord;
        neck = snakeBody[1].Coord;
        tail = snakeBody[snakeBody.Count - 1].Coord;
    }

    private void CopySnakeBody()
    {
        snakeBodyTemp.Clear();
        foreach (var item in snakeBody)
        {
            SnakeCell snakeCells = new SnakeCell();
            snakeCells.Coord = item.Coord;
            snakeCells.SpriteName = item.SpriteName;
            snakeBodyTemp.Add(snakeCells);
        }
    }
}
