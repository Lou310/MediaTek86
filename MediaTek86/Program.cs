namespace MediaTek86
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            new controleur.Controle();
        }
    }
}