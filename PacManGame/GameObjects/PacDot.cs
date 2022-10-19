namespace PacManGame.GameObjects;

public class PacDot : GameObject
{
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Yellow, XPosition, YPosition, Width, Height);
    }

    
}