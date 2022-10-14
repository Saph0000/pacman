namespace PacManGame.GameObjects.Ghosts;

public class Clyde : Ghost
{
    public Clyde() : base(325, 315, 50, 50)
    {
        speed = 5;
        image = baseImage = Image.FromFile(@"pictures\clyde.png");
        left = new[] { "clyde_Left (2)", "clyde_Left (1)" };
        right = new[] { "clyde_Right (2)", "clyde_Right (1)" };
        up = new[] { "clyde_Up (2)", "clyde_Up (1)" };
        down = new[]{ "clyde_Down (2)", "clyde_Down (1)"};
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

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, xPosition, yPosition, width, height);
    }
}