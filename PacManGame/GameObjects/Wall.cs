namespace PacManGame.GameObjects;

public class Wall : GameObject
{
    public bool Visible { get; set; }
    
    public override void Draw(PaintEventArgs e)
    {
        if (Visible)
            e.Graphics.FillRectangle(Brushes.DarkBlue, XPosition, YPosition, Width, Height);
    }
}