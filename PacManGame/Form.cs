using System.Text;
using PacManGame.Ghosts;

namespace PacManGame;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

public partial class Form1 : Form
{
    private Pacman pacman = new();
    
    private List<Ghost> ghosts = new()
    {
        new Blinky(),
        new Inky(),
        new Pinky(),
        new Clyde()
    };
    
    
    private List<Wall> walls = LevelFactory.Walls;
    private List<PacDot> pacDots = LevelFactory.PacDots;
    private List<PowerPallets> powerPallets = LevelFactory.PowerPallets;
    private int frame = 0;

    public Form1()
    {
        InitializeComponent();
        BackColor = Color.Black;
        DoubleBuffered = true;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Size = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
        this.CenterToScreen();
        this.KeyDown += HandleInput;
        Timer timer = new Timer();
        timer.Interval = 10;
        timer.Tick += TimerTick;
        timer.Enabled = true;
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
            ghost.Frightend();
        }

        pacman.CollectDots();
        pacman.CollectPowerPallets();
        
        frame++;
        if (frame == 5 || frame == 10)
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