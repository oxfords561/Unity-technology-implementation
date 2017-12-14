using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : MonoBehaviour {

    public Vector2 foodCoord;

    public void Init()
    {
        //snake.CreateFoodEvent += CreateFood;
    }

    public void CreateFood(GameController gameController)
    {
        foodCoord = GetRandomCoord(gameController);
    }

    Vector2 GetRandomCoord(GameController gameController)
    {
        
        bool isExist = true;
        Vector2 randomCoord = Vector2.zero;

        while (isExist)
        {
            int randomX = UnityEngine.Random.Range(0, 7);
            int randomY = UnityEngine.Random.Range(0, 15);
            randomCoord = new Vector2(randomX, randomY);
            for (int i = 0; i < gameController.snake.snakeBody.Count; i++)
            {
                if (gameController.snake.snakeBody[i].Coord != randomCoord)
                {
                    isExist = false;
                }
                else
                {
                    isExist = true;
                }
            }
        }

        return randomCoord;
    }
}
