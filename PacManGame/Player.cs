namespace PacManGame;

public class Player
{

    public Player()
    {
        Score = 0;
        Life = 3;
        Lose = false;
    }

  public int Life { get; set; }

  public long Score { get; set; }
  
  public bool Lose { get; set; }
}