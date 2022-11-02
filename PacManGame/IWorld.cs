﻿using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;

namespace PacManGame;

public interface IWorld
{
    List<PacDot> PacDots { get; }
    List<PowerPallet> PowerPallets { get; }
    List<Wall> Walls { get; }
    List<Fruit> Fruits { get; }
    List<Ghost> Ghosts { get; }
    Pacman Pacman { get; }
    Player Player { get; set; }
    Fruit Fruit { get; }
    Blinky Blinky {  get; }
    DateTime FrightenedStartTime { get; set; }
    DateTime GameStartTime { get; set; }
    public IDictionary<string, Image> ImageMap { get; }
    public int NextModeChangeTime { get; set; }
    public int ModeDurationIndex { get; set; }
    public int eatenGhosts { get; set; }

    public GhostMode CurrentGhostMode { get; set; }
}