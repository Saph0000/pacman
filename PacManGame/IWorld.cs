using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

namespace PacManGame;

public interface IWorld
{
    List<PacDot> PacDots { get; }
    List<PowerPallets> PowerPallets { get; }
    List<Wall> Walls { get; }
    List<Ghost> Ghosts { get; }
    Pacman Pacman { get; }
    Blinky Blinky {  get; }
    GhostMode GhostMode { get; set; }
}