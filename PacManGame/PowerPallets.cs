namespace PacManGame;

public class PowerPallets : GameObject
{
    public Brush Color { get; set; } = Brushes.Orange;
    
    public PowerPallets (int xPosition,int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }
    
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Color, xPosition, yPosition, this.width, this.height);
    }
}