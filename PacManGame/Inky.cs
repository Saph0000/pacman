namespace PacManGame;

public class Inky : Ghost
{
   
    public Inky() : base(325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\blinky.png");
        left = new string[] { "inky_Left (2)", "inky_Left (1)" };
        right = new string[] { "inky_Right (2)", "inky_Right (1)" };
        up = new string[] { "inky_Up (2)", "inky_Up (1)" };
        down = new string[]{ "inky_Down (2)", "inky_Down (1)"};
    }

   
    public void Chase(Pacman pacman, Ghost blinky)
    { 
        int pacmanXPosition;
        int pacmanYPosition;
        
        switch (pacman.viewangle)
        {
            case ViewAngle.Right:
            case ViewAngle.None:
                pacmanXPosition = pacman.xPosition + 50;
                pacmanYPosition = pacman.yPosition + 0;
                break;
            case ViewAngle.Left:
                pacmanXPosition = pacman.xPosition - 50;
                pacmanYPosition = pacman.yPosition + 0;
                break;
            case ViewAngle.Down:
                pacmanXPosition = pacman.xPosition + 0;
                pacmanYPosition = pacman.yPosition + 50;
                break;
            case ViewAngle.Up:
                pacmanXPosition = pacman.xPosition - 50;
                pacmanYPosition = pacman.yPosition - 50;
                break;
            default:
                pacmanXPosition = pacman.xPosition;
                pacmanYPosition = pacman.yPosition;
                break;
        }


        if (blinky.xPosition >= pacmanXPosition)
            targetXPosition = pacmanXPosition -  (blinky.xPosition - pacmanXPosition);
        if (blinky.yPosition >= pacmanYPosition)
            targetYPosition = pacmanYPosition - (blinky.yPosition - pacmanYPosition);
        if (blinky.xPosition <= pacmanXPosition)
            targetXPosition = pacmanXPosition + (pacmanXPosition - blinky.xPosition);
        if (blinky.yPosition <= pacmanYPosition)
            targetYPosition = pacmanYPosition + (pacmanYPosition - blinky.yPosition);
        
            
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Scatter()
    {
        targetXPosition = 660;
        targetYPosition = 850;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Frightend()
    {
        
    }
}