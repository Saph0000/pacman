namespace PacManGame.GameObjects.Ghosts;

public class Pinky : Ghost
{
    public Pinky(IWorld world) : base(world,325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"pictures\pinky.png");
        left = new[] { "pinky_Left (2)", "pinky_Left (1)" };
        right = new[] { "pinky_Right (2)", "pinky_Right (1)" };
        up = new[] { "pinky_Up (2)", "pinky_Up (1)" };
        down = new[]{ "pinky_Down (2)", "pinky_Down (1)"};
    }

   
    public override void Chase(Pacman pacman, Ghost blinky)
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

    public override void Scatter()
    {
        targetXPosition = 20;
        targetYPosition = 0;
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