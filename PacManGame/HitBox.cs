namespace PacManGame;

public class HitBox : GameObject
{
    public HitBox(GameObject gameObject, int padding) : base(gameObject.xPosition + padding, gameObject.yPosition + padding,
        gameObject.width - 2 * padding, gameObject.height - 2 * padding)
    {
    }

    public override void Draw(PaintEventArgs paintEventArgs)
    {
    }
}