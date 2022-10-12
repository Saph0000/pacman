namespace PacManGame;

public abstract class GameObject
{
    public int xPosition;
    public int yPosition;
    public int width;
    public int height;
    public int totalPacDots = 240;
    
    public Player player = new Player();

    protected GameObject(int xPosition, int yPosition, int width, int height)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
        this.width = width;
        this.height = height;
    }

    public bool HitObject(GameObject gameObject, GameObject testObject)
    {
        var hitbox = new HitBox(testObject, 5);
        return hitbox.xPosition < gameObject.xPosition + gameObject.width &&
               !(hitbox.xPosition < gameObject.xPosition) &&
               hitbox.yPosition < gameObject.yPosition + gameObject.height &&
               hitbox.yPosition + hitbox.height > gameObject.yPosition ||
               
                hitbox.xPosition + width > gameObject.xPosition &&
                !(hitbox.xPosition > gameObject.xPosition) &&
                hitbox.yPosition < gameObject.yPosition + gameObject.height &&
                hitbox.yPosition + hitbox.height > gameObject.yPosition ||
               
               hitbox.yPosition < gameObject.yPosition + gameObject.height &&
               !(hitbox.yPosition < gameObject.yPosition) &&
               hitbox.xPosition < gameObject.xPosition + gameObject.width && 
               hitbox.xPosition + hitbox.width > gameObject.xPosition ||
        
               hitbox.yPosition + hitbox.height > gameObject.yPosition &&
               !(hitbox.yPosition > gameObject.yPosition) &&
               hitbox.xPosition < gameObject.xPosition + gameObject.width && 
               hitbox.xPosition + hitbox.width > gameObject.xPosition;
    }

    public abstract void Draw(PaintEventArgs paintEventArgs);
}