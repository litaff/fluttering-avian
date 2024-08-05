namespace fluttering_avian;

using System;

public class RuntimeData
{
    public int Score { get; private set; }
    public bool ObstacleSpawned { get; set; }

    public void AddScore(int score)
    {
        if (score < 0)
        {
            throw new ArgumentException("[RuntimeData] Score must be positive!");
        }
        Score += score;
    }
    
    public void ResetScore()
    {
        Score = 0;
    }
}