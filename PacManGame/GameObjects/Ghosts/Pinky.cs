namespace PacManGame.GameObjects.Ghosts;

public class Pinky : Ghost
{
    public Pinky(IWorld world, int speed) : base(world,325, 375, 50, 50)
    {
        this.speed = speed;
        currentSpeed = speed;
    }

    protected override string ImageName => "pinky";

    protected override void Chase(Pacman pacman, Ghost blinky)
    {
        (targetXPosition, targetYPosition) = pacman.viewangle switch
        {
            ViewAngle.Left => (pacman.XPosition - 100, pacman.YPosition),
            ViewAngle.Down => (pacman.XPosition, pacman.YPosition + 100),
            ViewAngle.Up => (pacman.XPosition - 100, pacman.YPosition - 100),
            ViewAngle.Right or ViewAngle.None or _ => (pacman.XPosition + 100, pacman.YPosition),
        };
        GhostDecision(targetXPosition, targetYPosition);
    }

    protected override void Scatter()
    {
        targetXPosition = 20;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }
    
    public override void ReleaseGhost()
    {
        if (IsReleased) return;
        GhostMode = World.CurrentGhostMode;
        IsReleased = true;
    }
}