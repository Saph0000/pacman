namespace PacManGame.GameObjects;

public class HitBox : GameObject
{
    public HitBox(GameObject gameObject, int padding) 
    {
        XPosition = gameObject.XPosition + padding;
        YPosition = gameObject.YPosition + padding;
        Width = gameObject.Width - 2 * padding;
        Height = gameObject.Height - 2 * padding;
    }
}