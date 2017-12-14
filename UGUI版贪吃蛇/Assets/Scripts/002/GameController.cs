using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }

    public bool isLife = true;
    private GameObject gameOver;
    [HideInInspector]
    public Snake snake;
    [HideInInspector]
    public ChessBoard chessBoard;
    [HideInInspector]
    public Food food;
    private SnakeController snakeController;

	void Awake () {
        Instance = this;

        snake = GetComponent<Snake>();
        chessBoard = GetComponent<ChessBoard>();
        food = GetComponent<Food>();
        snakeController = GetComponent<SnakeController>();

        chessBoard.Init();
        snake.Init();
        food.Init();
        snakeController.Init();

        food.CreateFood(this);

        gameOver = transform.parent.Find("GameOver").gameObject;
       

        
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLife)
        {
            if (!gameOver.activeInHierarchy)
            {
                gameOver.SetActive(true);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                //游戏继续
                isLife = true;
                ResetGame();
            }
        }

	}

    void ResetGame()
    {
        if (gameOver.activeInHierarchy)
            gameOver.SetActive(false);
        //重置棋盘
        chessBoard.SetDefaultSpriteImage();
        //重置初始贪吃蛇信息
        snake.InitData();
    }
}
