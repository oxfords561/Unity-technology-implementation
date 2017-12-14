using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ChessBoard : MonoBehaviour
{

    [HideInInspector]
    //首先声明棋盘数组，存放image
    public Image[,] imgArray = new Image[7, 16];
    private GameController gameController;

    public void Init()
    {
        gameController = GameController.Instance;
        InitData();
    }

    private void InitData()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                imgArray[i, j] = transform.GetChild(i * 16 + j).GetComponent<Image>();
            }

        }
    }

    //设置棋盘具体坐标的图片
    public void SetSpriteImage(List<SnakeCell> snakeBody)
    {
        //设置其他格子
        SetDefaultSpriteImage();

        //设置food
        if (gameController.food.foodCoord != Vector2.zero)
            imgArray[(int)(gameController.food.foodCoord.x), (int)(gameController.food.foodCoord.y)].sprite = Resources.Load<Sprite>("002/Textures/Food");

        try
        {
            foreach (var item in snakeBody)
            {
                imgArray[(int)item.Coord.x, (int)item.Coord.y].sprite = Resources.Load<Sprite>("002/Textures/" + item.SpriteName);
            }
        }
        catch (System.Exception)
        {
            gameController.isLife = false;
            Debug.Log("游戏结束");
        }

        //判断贪吃蛇  吃食物
        if (gameController.snake.head != Vector2.zero && gameController.snake.head == gameController.food.foodCoord)
        {
            Vector2 direc = (gameController.snake.tail - gameController.snake.head).normalized;
            direc = new Vector2(direc.y, direc.x);
            if (Vector2.Dot(Vector2.up, direc) == 0)
            {
                if (gameController.snake.direction == Snake.Direction.Left)
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y + 1));
                }
                else if(gameController.snake.direction == Snake.Direction.Right)
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y - 1));
                }
            }else if(Vector2.Dot(Vector2.up, direc) == 1)
            {
                gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x + 1, gameController.snake.tail.y));
            }
            else if(Vector2.Dot(Vector2.up, direc) == -1)
            {
                gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x - 1, gameController.snake.tail.y));
            }
            else if (Vector2.Dot(Vector2.up, direc) < 0)
            {
                if(gameController.snake.tail.y < gameController.snake.head.y)
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y - 1));
                }
                else
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y + 1));
                }
            }
            else if (Vector2.Dot(Vector2.up, direc) > 0)
            {
                if (gameController.snake.tail.y < gameController.snake.head.y)
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y - 1));
                }
                else
                {
                    gameController.snake.AddSnakeBody(new Vector2(gameController.snake.tail.x, gameController.snake.tail.y + 1));
                }
            }


            gameController.food.CreateFood(gameController);
        }

    }

    /// <summary>
    /// 设置默认棋盘图片
    /// </summary>
    public void SetDefaultSpriteImage()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                imgArray[i, j].sprite = Resources.Load<Sprite>("002/Textures/透明");
            }
        }
    }

}
