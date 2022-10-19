using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

namespace PacManGame;

public interface IWorld
{
    List<PacDot> PacDots { get; }
    List<PowerPallet> PowerPallets { get; }
    List<Wall> Walls { get; }
    List<Ghost> Ghosts { get; }
    Pacman Pacman { get; }
    Blinky Blinky {  get; }
    DateTime FrightenedStartTime { get; set; }
}