namespace PacManGame;

static class Program
{
    [STAThread]
    static void Main()
    {
        //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnError);
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

    private static void OnError(object sender, UnhandledExceptionEventArgs e)
    {
        MessageBox.Show(e.ExceptionObject.ToString());
    }
}
