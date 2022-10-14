namespace PacManGame.GameObjects;

public class PacDot : GameObject
{
    public PacDot(int xPosition, int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Yellow, xPosition, yPosition, width, height);
    }

    
}