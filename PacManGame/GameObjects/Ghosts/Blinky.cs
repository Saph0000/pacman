﻿namespace PacManGame.GameObjects.Ghosts;

public class Blinky : Ghost
{
    public Blinky(IWorld world, float speed, float frightenedSpeed) : base(world,325, 315, 50, 50, frightenedSpeed)
    {
        this.speed = speed;
        this.frightenedSpeed = frightenedSpeed;
        currentSpeed = speed;
    }

    protected override string ImageName => "blinky";

    protected override void Chase(Pacman pacman, Ghost blinky)
    {
        targetXPosition = pacman.XPosition;
        targetYPosition = pacman.YPosition;
        GhostDecision(targetXPosition, targetYPosition);
    }

    protected override void Scatter()
    {
        targetXPosition = 660;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void ElroyMode()
    {
        speed += speed / 100 * 5;
    }
    
    public override void ReleaseGhost()
    {
        if (!IsReleased)
        {
            GhostMode = World.CurrentGhostMode;
            IsReleased = true;
        }
    }
}
