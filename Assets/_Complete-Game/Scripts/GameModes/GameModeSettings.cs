using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Mode", menuName = "Game Mode")]

public class GameModeSettings : ScriptableObject
{
    [Header("Player Settings")]
    [SerializeField] private int playerFoodPoints;
    [SerializeField] private int pointsPerFood = 10;
    [SerializeField] private int foodCap = 100;
    [Space]
    [Header("Enemy Settings")]
    [SerializeField] private int enemyDamage;
    [SerializeField] private List<int> enemiesPerLevel;
    [SerializeField] private bool avoidZeroEnemies = false;

    //TODO: Avoid negative positions
    [Header("Player and Exit Positions")]
    [SerializeField] private Vector2 exitPosition;
    [SerializeField] private Vector2 playerPosition;
    
    
    public int GetPlayerFoodPoint()
    {
        return playerFoodPoints;
    }
    
    public int GetEnemyDamagePoints()
    {
        return enemyDamage;
    }

    public int GetPointsPerFood()
    {
        return pointsPerFood;
    }

    //TODO: Limit the number of enemies to the size of the grid (8x8 by default) minus de obstacles and player 
    public int GetNumberOfEnemies(int level)
    {
        
        if (enemiesPerLevel.Count < level)
        {
            return  (int)Mathf.Log(level, 2f);
        }
        else
        {
            if (enemiesPerLevel[level-1] == 0 && avoidZeroEnemies)
            {
                return  (int)Mathf.Log(level, 2f);
            }
            return enemiesPerLevel[level-1];
        }
    }

    public Vector2 GetPlayerPosition(int x, int y)
    {
        return new Vector2(Mathf.Min((int)playerPosition.x, x) , Mathf.Min((int)playerPosition.y, y));
    }
    
    public Vector2 GetExitPosition(int x, int y)
    {
        return new Vector2(Mathf.Min((int) Mathf.Abs(exitPosition.x), x), Mathf.Min((int) Mathf.Abs(exitPosition.y), y));
    }

    public int GetFoodCap()
    {
        return foodCap;
    }
}
