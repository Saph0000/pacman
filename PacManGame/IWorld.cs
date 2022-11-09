using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

namespace PacManGame;

public interface IWorld
{
    List<PacDot> PacDots { get; set; }
    List<PowerPallet> PowerPallets { get; set; }
    List<Wall> Walls { get; }
    List<Fruit> Fruits { get; set; }
    List<Ghost> Ghosts { get; set; }
    Pacman Pacman { get; set; }
    Player Player { get; set; }
    Blinky Blinky {  get; set; }
    DateTime PacmanDeathTime { get; set; }
    DateTime FrightenedStartTime { get; set; }
    TimeSpan FrightenedTime { get; set; }
    DateTime GameStartTime { get; set; }
    DateTime PauseTime { get; set; }
    public bool IsPaused  { get; set; }
    public IDictionary<string, Image> ImageMap { get; }
    public TimeSpan NextModeChangeTime { get; set; }
    public int ModeDurationIndex { get; set; }
    public int eatenGhosts { get; set; }
    public int Highscore { get; set; }
    public Control Control { get; set; }
    public GhostMode CurrentGhostMode { get; set; }
}