namespace PacManGame.GameObjects;

public class HitBox : GameObject
{
    public int padding;
    public HitBox(GameObject gameObject,int padding)
    {
        this.padding = padding;
        XPosition = gameObject.XPosition + padding;
        YPosition = gameObject.YPosition + padding;
        Width = gameObject.Width - 2 * padding;
        Height = gameObject.Height - 2 * padding;
    }
}