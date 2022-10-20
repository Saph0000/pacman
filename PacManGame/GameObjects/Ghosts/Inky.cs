namespace PacManGame.GameObjects.Ghosts;

public class Inky : Ghost
{
   
    public Inky(IWorld world) : base(world,325, 375, 50, 50)
    {
        speed = 2;
        currentSpeed = speed;
    }

    protected override string ImageName => "inky";


    protected override void Chase(Pacman pacman, Ghost blinky)
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

    protected override void Scatter()
    {
        targetXPosition = 660;
        targetYPosition = 850;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}