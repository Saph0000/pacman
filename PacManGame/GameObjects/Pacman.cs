namespace PacManGame.GameObjects;

public class Pacman : GameActor
{
    //private HitBox HitBox;
    
    public Pacman(IWorld world) : base(world,330, 465, 50, 50)
    {
        speed = 3;
        //HitBox = new HitBox(2);
    }

    public HitBox HitBox => new(this, 15);

    public void CollectDots()
    {
        foreach (var dot in World.PacDots)
        {
            if (viewangle == ViewAngle.None || !HitBox.WouldOverlap(dot)) continue;
            World.PacDots.Remove(dot);
            World.Player.Score += 10;
            break;
        }

        if (!World.PacDots.Any())
        {
            //You cleared the level!!!
        }

    }

    public void CollectPowerPallets()
    {
        foreach (var powerPallet in World.PowerPallets.Where(powerPallet => viewangle != ViewAngle.None && HitBox.WouldOverlap(powerPallet)))
        {
            World.PowerPallets.Remove(powerPallet);
            World.FrightenedStartTime = DateTime.Now;
            World.NextModeChangeTime += 7;
            World.Player.Score += 50;
            foreach (var ghost in World.Ghosts.Where(ghost => ghost.GhostMode != GhostMode.Home && ghost.GhostMode != GhostMode.Off))
            {
                World.eatenGhosts = 0;
                ghost.GhostMode = GhostMode.Frightened;
                ghost.viewangle = ghost.viewangle.GetOppositeDirection();
                ghost.Frightened();
            }
            break;
        }
    }

    protected override string[] GetImageNames()
    {
        if (viewangle == ViewAngle.None)
            return new[] { "pacman_None", "pacman_None", "pacman_None" };
        return new[] { "pacman_None", "pacman_" + viewangle + " (1)", "pacman_" + viewangle + " (2)" };
    }

    public override void Die()
    {
        World.Player.Life--;
        if (World.Player.Life == 0)
            World.Player.Lose = true;
        XPosition = 330;
        YPosition = 465;
        foreach (var ghost in World.Ghosts)
        {
            ghost.XPosition = 325;
            ghost.YPosition = 375;
        }
    }
}