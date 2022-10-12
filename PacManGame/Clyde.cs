namespace PacManGame;

public class Clyde : Ghost
{
    public Clyde() : base(325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\clyde.png");
        left = new string[] { "clyde_Left (2)", "clyde_Left (1)" };
        right = new string[] { "clyde_Right (2)", "clyde_Right (1)" };
        up = new string[] { "clyde_Up (2)", "clyde_Up (1)" };
        down = new string[]{ "clyde_Down (2)", "clyde_Down (1)"};
    }

   
    public void Chase(Pacman pacman)
    {
        if (CalculateDistance(xPosition - pacman.xPosition, yPosition - pacman.yPosition) <= 200)
        {
            targetXPosition = 0;
            targetYPosition = 850;
        }
        else
        {
            targetXPosition = pacman.xPosition;
            targetYPosition = pacman.yPosition;
        }
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Scatter()
    {
        targetXPosition = 0;
        targetYPosition = 850;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Frightend()
    {
        
    }
}