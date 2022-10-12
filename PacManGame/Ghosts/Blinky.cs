namespace PacManGame.Ghosts;

public class Blinky : Ghost
{
    
    public Blinky() : base(325, 315, 50, 50)
    {
        speed = 2;
        image = baseImage = Image.FromFile(@"C:\Users\reutimann\RiderProjects\pacman\PacManGame\pictures\blinky.png");
        left = new[] { "blinky_Left (2)", "blinky_Left (1)" };
        right = new[] { "blinky_Right (2)", "blinky_Right (1)" };
        up = new[] { "blinky_Up (2)", "blinky_Up (1)" };
        down = new[]{ "blinky_Down (2)", "blinky_Down (1)"};
    }

    public void Chase(Pacman pacman)
    {
        targetXPosition = pacman.xPosition;
        targetYPosition = pacman.yPosition;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public void Scatter()
    {
        targetXPosition = 660;
        targetYPosition = 0;
        GhostDecision(targetXPosition, targetYPosition);
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, xPosition, yPosition, width, height);
    }
}
