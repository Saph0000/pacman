namespace PacManGame.GameObjects.Ghosts;

public class Pinky : Ghost
{
    public Pinky(IWorld world) : base(world,325, 375, 50, 50)
    {
        speed = 2;
        currentSpeed = speed;
    }

    protected override string ImageName => "pinky";

    protected override void Chase(Pacman pacman, Ghost blinky)
    {
        switch (pacman.viewangle)
        {
            case ViewAngle.Right:
            case ViewAngle.None:
                targetXPosition = pacman.XPosition + 100;
                targetYPosition = pacman.YPosition;
                break;
            case ViewAngle.Left:
                targetXPosition = pacman.XPosition - 100;
                targetYPosition = pacman.YPosition;
                break;
            case ViewAngle.Down:
                targetXPosition = pacman.XPosition;
                targetYPosition = pacman.YPosition + 100;
                break;
            case ViewAngle.Up:
                targetXPosition = pacman.XPosition - 100;
                targetYPosition = pacman.YPosition - 100;
                break;
        }
        
        GhostDecision(targetXPosition, targetYPosition);
    }

    protected override void Scatter()
    {
        targetXPosition = 20;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}