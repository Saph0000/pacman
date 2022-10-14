using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using PacManGame.GameObjects;
using PacManGame.GameObjects.Ghosts;
using Timer = System.Windows.Forms.Timer;

namespace PacManGame;

public class Window : Form
{
    private Pacman pacman = new();
    
    private List<Ghost> ghosts = new()
    {
        new Blinky(),
        new Inky(),
        new Pinky(),
        new Clyde(),
        
    };
    
    
    private List<Wall> walls = LevelFactory.Walls;
    private List<PacDot> pacDots = LevelFactory.PacDots;
    private List<PowerPallets> powerPallets = LevelFactory.PowerPallets;
    private int frame = 0;

    public Window()
    {
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        BackColor = Color.Black;
        DoubleBuffered = true;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Size = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
        CenterToScreen();
        KeyDown += HandleInput;
        Timer timer = new Timer();
        timer.Interval = 8;
        timer.Tick += TimerTick;
        timer.Enabled = true;;
    }

    private void TimerTick(object sender, EventArgs e)
    {
        pacman.CouldTurn(pacman.viewangle, pacman.nextViewangle);
        if (!pacman.WouldHitWall(pacman.nextViewangle))
        {
            pacman.viewangle = pacman.nextViewangle;
            pacman.nextViewangle = ViewAngle.None;
        }
        
        if (!pacman.WouldHitWall(pacman.viewangle))
            pacman.Move();

        foreach (var ghost in ghosts)
        {
            ghost.SetToNextTurn();
            ghost.Frightend(4, 8);
        }

        pacman.CollectDots();
        pacman.CollectPowerPallets();
        
        frame++;
        if (frame is 5 or 10 )
        {
            pacman.DrawActor(3);
        }

        if (frame == 10)
        {
            foreach (var ghost in ghosts)
                ghost.DrawActor(2);
            frame = 0;
        }
        

        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        foreach (var pacDot in pacDots)
            pacDot.Draw(e);
        e.Graphics.DrawString(pacman.player.Score.ToString(), Font, Brushes.Chartreuse, 10,10 );
        foreach (var wall in walls)
            wall.Draw(e);
        foreach (var powerPallet in powerPallets)
            powerPallet.Draw(e);
        foreach (var ghost in ghosts)
            ghost.Draw(e);
        pacman.Draw(e);
    }
    
    private void HandleInput(object sender, KeyEventArgs e)
    {
        pacman.nextViewangle = e.KeyCode switch
        {
            Keys.D or Keys.Right => ViewAngle.Right,
            Keys.A or Keys.Left => ViewAngle.Left,
            Keys.S or Keys.Down => ViewAngle.Down,
            Keys.W or Keys.Up => ViewAngle.Up,
            _ => pacman.nextViewangle
        };
    }
}