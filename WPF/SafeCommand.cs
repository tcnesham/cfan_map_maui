using Acr.UserDialogs;
using CFAN.SchoolMap.Services.Exceptions;

namespace CFAN.Common.WPF
{
    public class SafeCommand : Microsoft.Maui.Controls.Command
    {
        private static IExceptionHandler ExceptionHandler;
        static SafeCommand()
        {
            ExceptionHandler = DependencyService.Resolve<IExceptionHandler>();
        }
        public SafeCommand(Func<object, Task> execute, bool showLoading = true) : base(SafeAsync(execute, showLoading)) { }

        public SafeCommand(Func<Task> execute, bool showLoading = true) : base(SafeAsync(execute, showLoading)) { }

        public SafeCommand(Func<object, Task> execute, Func<object, bool> canExecute, bool showLoading = true) : base(SafeAsync(execute, showLoading), Safe(canExecute)) { }

        public SafeCommand(Func<Task> execute, Func<bool> canExecute, bool showLoading = true) : base(SafeAsync(execute, showLoading), Safe(canExecute)) { }

        public SafeCommand(Action<object> execute) : base(Safe(execute)) { }

        public SafeCommand(Action execute) : base(Safe(execute)) { }

        public SafeCommand(Action<object> execute, Func<object, bool> canExecute) : base(Safe(execute), Safe(canExecute)) { }

        public SafeCommand(Action execute, Func<bool> canExecute) : base(Safe(execute), Safe(canExecute)) { }

        private static Action<object> Safe(Action<object> execute)
        {
            return o =>
            {
                try
                {
                    execute(o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e,false,showMessage:false);
                }
            };
        }

        private static Action Safe(Action execute)
        {
            return () =>
            {
                try
                {
                    execute();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
            };
        }

        protected static Action<object> SafeAsync(Func<object, Task> execute, bool showLoading = true)
        {
            return async o =>
            {
                try
                {
                    if (showLoading) UserDialogs.Instance.ShowLoading();
                    await execute(o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                finally
                {
                    if (showLoading) UserDialogs.Instance.HideLoading();
                }
            };
        }

        protected static Action SafeAsync(Func<Task> execute, bool showLoading=true)
        {
            return async () =>
            {
                try
                {
                    if (showLoading) UserDialogs.Instance.ShowLoading();
                    await execute();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                finally
                {
                    if (showLoading) UserDialogs.Instance.HideLoading();
                }
            };
        }

        private static Func<object, bool> Safe(Func<object, bool> canExecute)
        {
            return o =>
            {
                try
                {
                    return canExecute(o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                return false;
            };
        }
        private static Func<bool> Safe(Func<bool> canExecute)
        {
            return () =>
            {
                try
                {
                    return canExecute();
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                return false;
            };
        }
    }

    public class SafeCommand<T> : Microsoft.Maui.Controls.Command
    {
        private static IExceptionHandler ExceptionHandler;
        static SafeCommand()
        {
            ExceptionHandler = DependencyService.Resolve<IExceptionHandler>();
        }
        public SafeCommand(Func<T, Task> execute, bool showLoading = true) : base(SafeAsync(execute, showLoading)) { }

        public SafeCommand(Func<T, Task> execute, Func<T, bool> canExecute, bool showLoading = true) : base(SafeAsync(execute, showLoading), Safe(canExecute)) { }

        public SafeCommand(Action<T> execute) : base(Safe(execute)) { }

        public SafeCommand(Action<T> execute, Func<T, bool> canExecute) : base(Safe(execute), Safe(canExecute)) { }

        private static Action<object> Safe(Action<T> execute)
        {
            return o =>
            {
                try
                {
                    execute((T)o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
            };
        }

        protected static Action<object> SafeAsync(Func<T, Task> execute, bool showLoading = true)
        {
            return async o =>
            {
                try
                {
                    if (showLoading) UserDialogs.Instance.ShowLoading();
                    await execute((T)o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                finally
                {
                    if (showLoading) UserDialogs.Instance.HideLoading();
                }
            };
        }

        private static Func<object, bool> Safe(Func<T, bool> canExecute)
        {
            return o =>
            {
                try
                {
                    return canExecute((T)o);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, showMessage: false);
                }
                return false;
            };
        }
    }
}
