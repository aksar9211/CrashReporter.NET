using System;
using System.Net;
using System.Windows.Forms;
using CrashReporterDotNET;

namespace CrashReporterTest
{
    static class Program
    {
        private static ReportCrash _reportCrash;

        /// <summary>
        /// The main entry point for the application.
        /// <summary>
        /// Initializes the application, configures crash reporting, registers global exception handlers, and starts the main form.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _reportCrash = new ReportCrash("Email where you want to receive crash reports")
            {
                Silent = true,
                ShowScreenshotTab = true,
                IncludeScreenshot = false,
                #region Optional Configuration
                //WebProxy = new WebProxy("Web proxy address, if needed"),
                //AnalyzeWithDoctorDump = true,
                //DoctorDumpSettings = new DoctorDumpSettings
                //{
                //    ApplicationID = new Guid("Application ID you received from DrDump.com"),
                //    OpenReportInBrowser = true
                //}
                #endregion
            };
            Application.ThreadException += (sender, args) => SendReport(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                                                          {
                                                            SendReport((Exception)args.ExceptionObject);
                                                          };
            _reportCrash.RetryFailedReports();
            Application.Run(new FormMain());
        }

    /// <summary>
        /// Sends a crash report for the specified exception, displaying the reporting UI to the user.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        /// <param name="developerMessage">An optional message to include for developers.</param>
        public static void SendReport(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = false;
            _reportCrash.Send(exception);
        }

        public static void SendReportSilently(Exception exception, string developerMessage = "")
        {
            _reportCrash.DeveloperMessage = developerMessage;
            _reportCrash.Silent = true;
            _reportCrash.Send(exception);
        }
    }
}
