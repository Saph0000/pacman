namespace PacManGame;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Windows.Forms.PropertyGridInternal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Form1 : Form
{
    private Pacman pacman = new();
    private Blinky blinky = new();
    private Inky inky = new();
    private Pinky pinky = new();
    private Clyde clyde = new();
    
    
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
        
        blinky.SetToNextTurn();
        inky.SetToNextTurn();
        pinky.SetToNextTurn();
        clyde.SetToNextTurn();
        blinky.Frightend();
        inky.Frightend();
        pinky.Frightend();
        clyde.Frightend();
        
        pacman.CollectDots();
        pacman.CollectPowerPallets();
        
        /*if (pacman.HitObject(blinky, pacman) || (pacman.HitObject(inky, pacman)))
        {
            pacman.Die();
            //pacman.Restart();
            pacman.xPosition = 325;
            pacman.yPosition = 465;
            inky.xPosition = blinky.xPosition = 325;
            inky.yPosition = blinky.yPosition = 360;
            pacman.viewangle = ViewAngle.None;
            System.Windows.Forms.MessageBox.Show("You died!!!");
        }*/

        frame++;
        if (frame == 10)
        {
            pacman.DrawActor(3);
            blinky.DrawActor(2);
            inky.DrawActor(2);
            pinky.DrawActor(2);
            clyde.DrawActor(2);
            
            frame = 0;
        }

        Invalidate();
    }

   

    protected override void OnPaint(PaintEventArgs e)
    {
        foreach (var pacDot in pacDots)
            pacDot.MakeDot(e);
        e.Graphics.DrawString((pacman.player.Score).ToString(), Font, Brushes.Chartreuse, 10,10 );
        
        foreach (var wall in walls)
            wall.MakeWall(e);
        foreach (var powerPallet in powerPallets)
            powerPallet.MakeWall(e);
        e.Graphics.DrawImage(blinky.image, blinky.xPosition, blinky.yPosition, blinky.width, blinky.height);
        e.Graphics.DrawImage(inky.image, inky.xPosition, inky.yPosition, inky.width, inky.height);
        e.Graphics.DrawImage(pinky.image, pinky.xPosition, pinky.yPosition, pinky.width, pinky.height);
        e.Graphics.DrawImage(clyde.image, clyde.xPosition, clyde.yPosition, clyde.width, clyde.height);
        e.Graphics.DrawImage(pacman.image, pacman.xPosition, pacman.yPosition, pacman.width, pacman.height);
        
        e.Graphics.FillEllipse(Brushes.Red, blinky.targetXPosition, blinky.targetYPosition, 20,20);
        e.Graphics.FillEllipse(Brushes.Aqua, inky.targetXPosition, inky.targetYPosition, 20,20);
        e.Graphics.FillEllipse(Brushes.Pink, pinky.targetXPosition, pinky.targetYPosition, 20,20);
        e.Graphics.FillEllipse(Brushes.Orange, clyde.targetXPosition, clyde.targetYPosition, 20,20);
        
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