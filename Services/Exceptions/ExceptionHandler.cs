using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Maui.Services.Exceptions;
using CFAN.SchoolMap.Services.Exceptions;

[assembly: Dependency(typeof(ExceptionHandler))]
namespace CFAN.SchoolMap.Maui.Services.Exceptions
{
    /// <summary>
    /// Zpracování chyb
    /// </summary>
    public class ExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Zpracování chyby
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="fatal">if set to <c>true</c> [fatal].</param>
        /// <param name="afterException">Akce po zobrazení dialogu s chybou</param>
        public async void HandleException(Exception exception, bool fatal, Action afterException=null, string message = null, bool showMessage=true)
        {
            exception = exception.InnerException ?? exception;
            try
            {
                string exceptionMessage = exception.ToString();
                if (exceptionMessage.Contains("WebException"))
                {
                    await ShowMessage("Connection to server failed.", showMessage);
                }
                else
                {
#if DEBUG
                    message = exceptionMessage;
#endif
                    try
                    {
                        var repo = DependencyService.Resolve<IRepository>();
                        if (DependencyService.Resolve<IAuth>().IsAdmin)
                        {
                            message = message ?? "Error occured in the application";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    
                    await ShowMessage(message, showMessage);
                }
                afterException?.Invoke();
                WriteExceptionLog(exception);
            }
            catch{}

            
        }

        private async Task ShowMessage(string message, bool showDialog)
        {
            if (showDialog)
            {
                await UserDialogs.Instance.AlertAsync(message);
            }
            else
            {
                UserDialogs.Instance.Toast(message);
            }
        }

        public void WriteExceptionLog(Exception exception)
        {
            var logMessage = $"{DateTime.Now}\n{exception.Message}\n{exception.StackTrace}";
#if DEBUG
            Debug.WriteLine(logMessage);
#endif

            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "errors.txt");
                File.WriteAllText(path, logMessage);
            }
            catch { }
        }
    }
}
