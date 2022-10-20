using PacManGame.Helper;

namespace PacManGame.GameObjects.Ghosts;

public class Clyde : Ghost
{
    public Clyde(IWorld world) : base(world, 325, 375, 50, 50)
    {
        speed = 2;
        currentSpeed = speed;
    }
    
    protected override string ImageName => "clyde";


    public override void Chase(Pacman pacman, Ghost blinky)
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

    public override void Scatter()
    {
        targetXPosition = 0;
        targetYPosition = 850;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}