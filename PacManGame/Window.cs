using Timer = System.Windows.Forms.Timer;

namespace PacManGame;

public class Window : Form
{
    private readonly World world;
    
    public Window()
    {
        world = new World();
        Setup();
        CenterToScreen();
        KeyDown += HandleInput;
        var timer = CreateTimer();
    }

    private void Setup()
    {
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        BackColor = Color.Black;
        DoubleBuffered = true;
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Size = new Size(Settings.ScreenWidth, Settings.ScreenHeight);
    }

    private Timer CreateTimer()
    {
        var timer = new Timer();
        timer.Interval = 8;
        timer.Tick += TimerTick;
        timer.Enabled = true;
        return timer;
    }

    private void TimerTick(object sender, EventArgs e)
    {
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        world.Draw(e);
        world.Tick(e);
    }

    private void HandleInput(object sender, KeyEventArgs e)
    {
        world.Pacman.nextViewangle = e.KeyCode switch
        {
            Keys.D or Keys.Right => ViewAngle.Right,
            Keys.A or Keys.Left => ViewAngle.Left,
            Keys.S or Keys.Down => ViewAngle.Down,
            Keys.W or Keys.Up => ViewAngle.Up,
            _ => world.Pacman.nextViewangle
        };
        world.Control = e.KeyCode switch
        {
            Keys.Escape => Control.Esc,
            Keys.Enter => Control.Enter,
            _ =>world.Control
        };
    }
}