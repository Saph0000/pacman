namespace PacManGame;

public class Wall : GameObject
{
    public Brush Color { get; set; } = Brushes.Blue;
    
    public Wall (int xPosition,int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }

    public void MakeWall(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Color, xPosition, yPosition, this.width, this.height);
    }
}