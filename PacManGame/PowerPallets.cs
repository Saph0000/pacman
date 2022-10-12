namespace PacManGame;

public class PowerPallets : GameObject
{
    public PowerPallets (int xPosition,int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }
    
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Orange, xPosition, yPosition, this.width, this.height);
    }
}