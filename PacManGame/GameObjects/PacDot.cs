namespace PacManGame.GameObjects;

public class PacDot : GameObject
{
    public PacDot(IWorld world, int xPosition, int yPosition, int width, int height) : base(world, xPosition, yPosition, width, height)
    {
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Yellow, XPosition, YPosition, Width, Height);
    }

    
}