﻿namespace PacManGame;

public class Player
{

    public Player()
    {
        Score = 0;
        Life = 3;
        Lose = false;
        Level = 1;
    }

  public int Life { get; set; }
  
  public long AddedScore 
  { 
      get;
      set; 
  }
  
  public long Score { get; set; }
  
  public bool Lose { get; set; }
  
  public int Level { get; set; }
}