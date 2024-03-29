﻿namespace PacManGame.GameObjects;

public class Pacman : GameActor
{
    //private HitBox HitBox;
    
    public Pacman(IWorld world, float speed) : base(world,325, 615, 50, 50)
    {
        this.speed = speed;
        //HitBox = new HitBox(2);
    }

    public HitBox HitBox => new(this, 15);

    public void CollectPacDots()
    {
        foreach (var dot in World.PacDots )
        {
            if (viewangle == ViewAngle.None || !HitBox.WouldOverlap(dot)) continue;
            World.PacDots.Remove(dot);
            World.Player.Score += 10;
            break;
        }
    }

    public void CollectFruits()
    {
        foreach (var fruit in World.Fruits)
        {
            if (viewangle == ViewAngle.None || !HitBox.WouldOverlap(fruit) || (World.PacDots.Count >= 170)) continue;
            World.Fruits.Remove(fruit);
            World.Player.Score += 100;
            break;
        }
    }

    public void CollectPowerPallets()
    {
        foreach (var powerPallet in World.PowerPallets.Where(powerPallet => viewangle != ViewAngle.None && HitBox.WouldOverlap(powerPallet)))
        {
            World.PowerPallets.Remove(powerPallet);
            World.FrightenedStartTime = DateTime.Now;
            World.NextModeChangeTime += World.FrightenedTime;
            World.Player.Score += 50;
            foreach (var ghost in World.Ghosts.Where(ghost => ghost.GhostMode != GhostMode.Home && ghost.GhostMode != GhostMode.Off))
            {
                World.eatenGhosts = 0;
                ghost.GhostMode = GhostMode.Frightened;
                ghost.viewangle = ghost.viewangle.GetOppositeDirection();
                ghost.Frightened();
            }
            break;
        }
    }

    protected override string[] GetImageNames()
    {
        return viewangle == ViewAngle.None ? new[] { "pacman_None", "pacman_None", "pacman_None" } : new[] { "pacman_None", "pacman_" + viewangle + " (1)", "pacman_" + viewangle + " (2)" };
    }

    public override void Die()
    {
        World.PacmanDeathTime = DateTime.Now + TimeSpan.FromSeconds(2);
        World.Player.Life--;
        if (World.Player.Life == 0)
            World.Player.Lose = true;
        XPosition = 325;
        YPosition = 615;
        foreach (var ghost in World.Ghosts)
        {
            ghost.XPosition = 325;
            ghost.YPosition = 375;
        }
    }
}