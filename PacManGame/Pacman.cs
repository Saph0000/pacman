using Timer = System.Windows.Forms.Timer;

namespace PacManGame;

public class Pacman : GameActor
{
    private bool isDead;

    public Pacman() : base(330, 465, 50, 50)
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
        foreach (var dot in LevelFactory.PacDots)
        {
            
            if (viewangle != ViewAngle.None && WouldOverlap(dot))
            {
                LevelFactory.PacDots.Remove(dot);
                player.Score += 10;
                totalPacDots--;
                break;
            }
        }
    }

    public void CollectPowerPallets()
    {
        foreach (var powerPallet in LevelFactory.PowerPallets)
        {
            
            if (viewangle != ViewAngle.None && WouldOverlap(powerPallet))
            {
                LevelFactory.PowerPallets.Remove(powerPallet);
                player.Score += 50;
                break;
            }
        }
    }

    public override void Draw(PaintEventArgs e)
    {
        e.Graphics.DrawImage(image, xPosition, yPosition, width, height);
    }
}