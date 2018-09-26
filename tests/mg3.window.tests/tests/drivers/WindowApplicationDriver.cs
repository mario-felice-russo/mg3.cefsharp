using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace mg3.drivers
{
    public class WindowApplicationDriver : ISeleniumWindowDriver, IDisposable
    {
        public WindowApplicationDriver() { }

        private string Host;
        private int Port;

        private WindowsDriver<WindowsElement> driver = null;
        private Process winappdriver = null;
        private Process inspector = null;

        public WindowsDriver<WindowsElement> Get(string host = "http://127.0.0.1", int port = 4723, string application = null, string onsystem = null)
        {
            Host = host;
            Port = port;

            if (!(string.IsNullOrEmpty(application) || string.IsNullOrEmpty(onsystem)))
            {
                try
                {
                    if (IsNotAlreadyRunning() || winappdriver == null)
                    {
                        winappdriver = RunProcess("WinAppDriver.exe", @"C:\Program Files (x86)\Windows Application Driver\");

                        if (winappdriver != null)
                        {
                            DesiredCapabilities appCapabilities = new DesiredCapabilities();
                            appCapabilities.SetCapability("app", application);
                            appCapabilities.SetCapability("device", onsystem);

                            driver = new WindowsDriver<WindowsElement>(new Uri($"{Host}:{Port}"), appCapabilities);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error - message    -> " + e.Message);
                    Debug.WriteLine("Error - stacktrace -> " + e.StackTrace);
                    Close();
                }
                finally
                {
                    if (driver != null) driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
                }

                if (Debugger.IsAttached)
                {
                    Console.WriteLine(@"UI inspector at directories (if windows SDK is installed):");
                    Console.WriteLine(@"C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x86\inspect.exe");
                    Console.WriteLine(@"C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\inspect.exe");
                }
            }

            return driver;
        }

        public bool IsNotAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            bool bCreatedNew;

            Mutex mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew);
            if (bCreatedNew)
                mutex.ReleaseMutex();

            return bCreatedNew;
        }

        public Process RunProcess(string filename, string filepath)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(filepath + filename);
            p.StartInfo.WorkingDirectory = filepath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;

            if (File.Exists(filepath + filename))
                p.Start();

            return p;
        }

        public bool Close()
        {
            bool result = false;

            if (result = driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }

            if (result = winappdriver != null && !winappdriver.HasExited)
            {
                winappdriver.CloseMainWindow();
                winappdriver = null;
            }

            if (result = inspector != null)
            {
                inspector.Close();
            }

            return result;
        }

        #region IDisposable Support

        private bool disposedValue = false; // Per rilevare chiamate ridondanti

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminare lo stato gestito (oggetti gestiti).
                    Close();
                }

                // TODO: liberare risorse non gestite (oggetti non gestiti) ed eseguire sotto l'override di un finalizzatore.
                // TODO: impostare campi di grandi dimensioni su Null.

                disposedValue = true;
            }
        }

        // TODO: eseguire l'override di un finalizzatore solo se Dispose(bool disposing) include il codice per liberare risorse non gestite.
        // ~WindowApplicationDriver() {
        //   // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
        //   Dispose(false);
        // }

        // Questo codice viene aggiunto per implementare in modo corretto il criterio Disposable.
        public void Dispose()
        {
            // Non modificare questo codice. Inserire il codice di pulizia in Dispose(bool disposing) sopra.
            Dispose(true);
            // TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override del finalizzatore.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
