namespace PacManGame;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnError);
        ApplicationConfiguration.Initialize();
        Application.Run(new Window());
    }

    private static void OnError(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show(e.ExceptionObject.ToString());
    }
}
