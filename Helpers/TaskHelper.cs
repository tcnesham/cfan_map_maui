using System;
using System.Threading;
using System.Threading.Tasks;
using CFAN.SchoolMap.Services.Exceptions;
using Microsoft.Maui.Controls;

namespace CFAN.SchoolMap.Helpers
{
    public static class TaskHelper
    {
        private static readonly IExceptionHandler ExceptionHandler;

        static TaskHelper()
        {
            ExceptionHandler = DependencyService.Get<IExceptionHandler>();
        }

        public static async Task SafeRun(Task task, bool showMessage = true)
       { 
            try
            {
                await task;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, false, showMessage:showMessage);
            }
        }

        public static async Task SafeRun(Func<Task> task, bool showMessage=true)
        {
            try
            {
                await task();
            }
            catch (Exception e)
            {
                if (showMessage)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: showMessage);
                }
            }
        }

        public static async Task<bool> RunWithTimeout(this Task task, int seconds=10, Action onError=null)
        {
            using var cts = new CancellationTokenSource(seconds * 1000);
            var awaited = Task.Run(() => task, cts.Token);
            try
            {
                if (await Task.WhenAny(awaited, Task.Delay(seconds * 1000, cts.Token)) == awaited)
                {
                    await awaited;
                    cts.Cancel();
                    return true;
                }
                else
                {
                    cts.Cancel();
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
