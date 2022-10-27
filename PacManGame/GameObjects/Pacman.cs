﻿namespace PacManGame.GameObjects;

public class Pacman : GameActor
{
    public Pacman(IWorld world) : base(world,330, 465, 50, 50)
    {
        speed = 7;
    }

    public void CollectDots()
    {
        foreach (var dot in World.PacDots)
        {
            if (viewangle == ViewAngle.None || !WouldOverlap(dot)) continue;
            World.PacDots.Remove(dot);
            //player.Score += 10;
            break;
        }

        if (!World.PacDots.Any())
        {
            //You cleared the level!!!
        }

    }

    public void CollectPowerPallets()
    {
        foreach (var powerPallet in World.PowerPallets.Where(powerPallet => viewangle != ViewAngle.None && WouldOverlap(powerPallet)))
        {
            World.PowerPallets.Remove(powerPallet);
            World.FrightenedStartTime = DateTime.Now;
            World.NextModeChangeTime += 7;
            //player.Score += 50;
            foreach (var ghost in World.Ghosts.Where(ghost => ghost.GhostMode != GhostMode.Home && ghost.GhostMode != GhostMode.Off))
            {
                ghost.GhostMode = GhostMode.Frightened;
                ghost.viewangle = ghost.viewangle.GetOppositeDirection();
                ghost.Frightened();
            }
            break;
        }
    }

    protected override string[] GetImageNames()
    {
        if (viewangle == ViewAngle.None)
            return new[] { "pacman_None", "pacman_None", "pacman_None" };
        return new[] { "pacman_None", "pacman_" + viewangle + " (1)", "pacman_" + viewangle + " (2)" };
    }

    public void Die()
    {
        //lives--;
        //if (lives = 4)
        //    Loose = true;
    }
}