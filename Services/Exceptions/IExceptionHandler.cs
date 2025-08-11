using System;

namespace CFAN.SchoolMap.Services.Exceptions
{
    /// <summary>
    /// rozhraní pro zpracování chyb
    /// </summary>
    public interface IExceptionHandler
    {
        void HandleException(Exception exception, bool fatal, Action afterException = null, string message = null, bool showMessage = true);
        void WriteExceptionLog(Exception exception);
    }
}
