namespace PacManGame.GameObjects.Ghosts;

public class Pinky : Ghost
{
    public Pinky() : base(325, 315, 50, 50)
    {
        speed = 10;
        image = baseImage = Image.FromFile(@"pictures\pinky.png");
        left = new[] { "pinky_Left (2)", "pinky_Left (1)" };
        right = new[] { "pinky_Right (2)", "pinky_Right (1)" };
        up = new[] { "pinky_Up (2)", "pinky_Up (1)" };
        down = new[]{ "pinky_Down (2)", "pinky_Down (1)"};
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

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, xPosition, yPosition, width, height);
    }
}