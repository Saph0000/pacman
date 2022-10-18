namespace PacManGame.GameObjects.Ghosts;

public class Inky : Ghost
{
   
    public Inky(IWorld world) : base(world,325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"pictures\blinky.png");
        left = new[] { "inky_Left (2)", "inky_Left (1)" };
        right = new[] { "inky_Right (2)", "inky_Right (1)" };
        up = new[] { "inky_Up (2)", "inky_Up (1)" };
        down = new[]{ "inky_Down (2)", "inky_Down (1)"};
    }

   
    public override void Chase(Pacman pacman, Ghost blinky)
    { 
        int pacmanXPosition;
        int pacmanYPosition;
        
        switch (pacman.viewangle)
        {
            case ViewAngle.Right:
            case ViewAngle.None:
                pacmanXPosition = pacman.XPosition + 50;
                pacmanYPosition = pacman.YPosition + 0;
                break;
            case ViewAngle.Left:
                pacmanXPosition = pacman.XPosition - 50;
                pacmanYPosition = pacman.YPosition + 0;
                break;
            case ViewAngle.Down:
                pacmanXPosition = pacman.XPosition + 0;
                pacmanYPosition = pacman.YPosition + 50;
                break;
            case ViewAngle.Up:
                pacmanXPosition = pacman.XPosition - 50;
                pacmanYPosition = pacman.YPosition - 50;
                break;
            default:
                pacmanXPosition = pacman.XPosition;
                pacmanYPosition = pacman.YPosition;
                break;
        }


        if (blinky.XPosition >= pacmanXPosition)
            targetXPosition = pacmanXPosition -  (blinky.XPosition - pacmanXPosition);
        if (blinky.YPosition >= pacmanYPosition)
            targetYPosition = pacmanYPosition - (blinky.YPosition - pacmanYPosition);
        if (blinky.XPosition <= pacmanXPosition)
            targetXPosition = pacmanXPosition + (pacmanXPosition - blinky.XPosition);
        if (blinky.YPosition <= pacmanYPosition)
            targetYPosition = pacmanYPosition + (pacmanYPosition - blinky.YPosition);
        
            
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Scatter()
    {
        targetXPosition = 660;
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