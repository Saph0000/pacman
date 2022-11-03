using PacManGame.Helper;

namespace PacManGame.GameObjects.Ghosts;

public class Clyde : Ghost
{
    public Clyde(IWorld world, int speed) : base(world, 325, 375, 50, 50)
    {
        this.speed = speed;
        currentSpeed = speed;
    }
    
    protected override string ImageName => "clyde";


    protected override void Chase(Pacman pacman, Ghost blinky)
    {
        if (Maths.CalculateDistance(XPosition, YPosition, pacman.XPosition, pacman.YPosition) <= 200)
        {
            targetXPosition = 0;
            targetYPosition = 850;
        }
        else
        {
            targetXPosition = pacman.XPosition;
            targetYPosition = pacman.YPosition;
        }
        GhostDecision(targetXPosition, targetYPosition);
    }

    protected override void Scatter()
    {
        targetXPosition = 0;
        targetYPosition = 850;
        GhostDecision(targetXPosition, targetYPosition);
    }
    public override void ReleaseGhost()
    {
        if (World.PacDots.Count <= 150 && !IsReleased)
        {
            GhostMode = World.CurrentGhostMode;
            IsReleased = true;
        }


    }
}