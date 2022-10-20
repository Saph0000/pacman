namespace PacManGame.GameObjects.Ghosts;

public class Blinky : Ghost
{
    public Blinky(IWorld world) : base(world,325, 375, 50, 50)
    {
        speed = 2;
        currentSpeed = speed;
    }

    protected override string ImageName => "blinky";
    
    public override void Chase(Pacman pacman, Ghost blinky)
    {
        targetXPosition = pacman.XPosition;
        targetYPosition = pacman.YPosition;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Scatter()
    {
        targetXPosition = 660;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }


    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}
