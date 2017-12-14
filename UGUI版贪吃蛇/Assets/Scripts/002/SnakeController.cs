using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour {

    private Snake snake;
    private GameController gameController;

	public void Init () {
        snake = GetComponent<Snake>();
        gameController = GameController.Instance;
	}
	
	void Update () {

        if (gameController && gameController.isLife)
        {
            ControllSnakeInput();
        }
        

    }

    void ControllSnakeInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (snake.direction == Snake.Direction.Down)
                gameController.isLife = false;
            snake.direction = Snake.Direction.Up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (snake.direction == Snake.Direction.Right)
                gameController.isLife = false;
            snake.direction = Snake.Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (snake.direction == Snake.Direction.Up)
                gameController.isLife = false;
            snake.direction = Snake.Direction.Down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (snake.direction == Snake.Direction.Left)
                gameController.isLife = false;
            snake.direction = Snake.Direction.Right;
        }
    }
}
