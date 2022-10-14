namespace PacManGame.GameObjects;

public class Wall : GameObject
{
    public Wall (int xPosition,int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.DarkBlue, xPosition, yPosition, this.width, this.height);
    }
}