using System;
using System.Reflection;
using System.Windows.Forms;

namespace POCOGeneratorUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // UI exceptions
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

                // Non-UI exceptions
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => UnhandledException((Exception)e.ExceptionObject);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new POCOGeneratorForm());
            }
            catch (Exception ex)
            {
                UnhandledException(ex);
            }
        }

        private static void UnhandledException(Exception ex)
        {
            try
            {
                MessageBox.Show(
                    ex.GetUnhandledExceptionErrorMessage(),
                    string.Format("Unhandled Error - {0} {1}",
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Assembly.GetExecutingAssembly().GetName().Version.ToString(3)),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch
            {
            }

            Application.Exit();
        }
    }
}
