namespace PacManGame.Levels;

public class Level
{
    public int[] Levels { get; set; }
    public float PacmanSpeed { get; set; }
    public float GhostSpeed { get; set; }
    public int ElroyModeCounter { get; set; }
    public string[] Fruits{ get; set; }
    public float FrightenedTime { get; set; }
    public float FrightenedSpeed { get; set; }
    public int[] GhostModeTime { get; set; }
}