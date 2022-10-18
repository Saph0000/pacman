namespace PacManGame.GameObjects.Ghosts;

public class Clyde : Ghost
{
    public Clyde(IWorld world) : base(world, 325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"pictures\clyde.png");
        left = new[] { "clyde_Left (2)", "clyde_Left (1)" };
        right = new[] { "clyde_Right (2)", "clyde_Right (1)" };
        up = new[] { "clyde_Up (2)", "clyde_Up (1)" };
        down = new[]{ "clyde_Down (2)", "clyde_Down (1)"};
    }

   
    public override void Chase(Pacman pacman, Ghost blinky)
    {
        if (CalculateDistance(XPosition - pacman.XPosition, YPosition - pacman.YPosition) <= 200)
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

    public void Frightend()
    {
        
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}