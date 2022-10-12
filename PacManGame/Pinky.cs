namespace PacManGame;

public class Pinky : Ghost
{
    public Pinky() : base(325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"C:\Users\reutimann\RiderProjects\PacManGame\PacManGame\pictures\pinky.png");
        left = new string[] { "pinky_Left (2)", "pinky_Left (1)" };
        right = new string[] { "pinky_Right (2)", "pinky_Right (1)" };
        up = new string[] { "pinky_Up (2)", "pinky_Up (1)" };
        down = new string[]{ "pinky_Down (2)", "pinky_Down (1)"};
    }

   
    public void Chase(Pacman pacman)
    {
        switch (pacman.viewangle)
        {
            case ViewAngle.Right:
            case ViewAngle.None:
                targetXPosition = pacman.xPosition + 100;
                targetYPosition = pacman.yPosition;
                break;
            case ViewAngle.Left:
                targetXPosition = pacman.xPosition - 100;
                targetYPosition = pacman.yPosition;
                break;
            case ViewAngle.Down:
                targetXPosition = pacman.xPosition;
                targetYPosition = pacman.yPosition + 100;
                break;
            case ViewAngle.Up:
                targetXPosition = pacman.xPosition - 100;
                targetYPosition = pacman.yPosition - 100;
                break;
        }
        
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Scatter()
    {
        targetXPosition = 20;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Frightend()
    {
        
    }
}