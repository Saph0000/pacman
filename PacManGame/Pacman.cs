﻿using Timer = System.Windows.Forms.Timer;

namespace PacManGame;

public class Pacman : GameActor
{
    private bool isDead;




    public Pacman() : base(330, 465, 50, 50)
    {
        speed = 7;
        image = baseImage = Image.FromFile(@"C:\Users\reutimann\RiderProjects\PacManGame\PacManGame\pictures\pacman.png");
        left = new string[] { "pacman", "pacman_Left (1)", "pacman_Left (2)" };
        right = new string[] { "pacman", "pacman_Right (1)", "pacman_Right (2)" };
        up = new string[] { "pacman", "pacman_Up (1)", "pacman_Up (2)" };
        down = new string []{ "pacman", "pacman_Down (1)", "pacman_Down (2)"};
    }

    public void CollectDots()
    {
        foreach (var dot in LevelFactory.PacDots)
        {
            
            if (viewangle != ViewAngle.None && HitObject(dot, this))
            {
                LevelFactory.PacDots.Remove(dot);
                player.Score += 10;
                break;
            }
        }
    }
}