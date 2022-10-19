namespace PacManGame.GameObjects;

public class PowerPallet : GameObject
{
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Orange, XPosition, YPosition, Width, Height);
    }
}