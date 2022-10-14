namespace PacManGame.GameObjects;

public class PowerPallets : GameObject
{
    public PowerPallets (IWorld world, int xPosition,int yPosition, int width, int height) : base(world, xPosition, yPosition, width, height)
    {
    }
    
    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillEllipse(Brushes.Orange, XPosition, YPosition, this.Width, this.Height);
    }
}