namespace PacManGame;

public class Intersection : GameObject
{
    public Intersection(int xPosition, int yPosition, int width, int height) : base(xPosition, yPosition, width, height)
    {
    }
    public void MakeIntersection(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.GreenYellow, xPosition, yPosition, this.width, this.height);
    }
}