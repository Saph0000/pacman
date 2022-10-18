namespace PacManGame.GameObjects;

public class Wall : GameObject
{
    public Wall (IWorld world, int xPosition,int yPosition, int width, int height) : base(world, xPosition, yPosition, width, height)
    {
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.DarkBlue, XPosition, YPosition, Width, Height);
    }
}