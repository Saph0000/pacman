namespace PacManGame.GameObjects;

public class Pacman : GameActor
{
    private bool isDead;

    public Pacman(IWorld world) : base(world,330, 465, 50, 50)
    {
        speed = 7;
        image = baseImage = Image.FromFile(@"pictures\pacman.png");
        left = new[] { "pacman", "pacman_Left (1)", "pacman_Left (2)" };
        right = new[] { "pacman", "pacman_Right (1)", "pacman_Right (2)" };
        up = new[] { "pacman", "pacman_Up (1)", "pacman_Up (2)" };
        down = new[]{ "pacman", "pacman_Down (1)", "pacman_Down (2)"};
    }

    public void CollectDots()
    {
        foreach (var dot in World.PacDots)
        {
            
            if (viewangle != ViewAngle.None && WouldOverlap(dot))
            {
                World.PacDots.Remove(dot);
                //player.Score += 10;
                break;
            }
        }
    }

    public void CollectPowerPallets()
    {
        foreach (var powerPallet in World.PowerPallets)
        {
            
            if (viewangle != ViewAngle.None && WouldOverlap(powerPallet))
            {
                World.PowerPallets.Remove(powerPallet);
                //player.Score += 50;
                break;
            }
        }
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, XPosition, YPosition, Width, Height);
    }
}