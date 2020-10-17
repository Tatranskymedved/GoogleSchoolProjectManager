using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GoogleSchoolProjectManager.UI.ViewModel
{
    public interface ICommand<T> : ICommand
    {
    }

    public abstract class RelayCommandBase2<T> : ICommand, INotifyPropertyChanged, ICommand<T>
    {
        #region Fields & Properties

        //private static readonly ILog mLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Action<T> mAction;
        private readonly Func<T, Task> mFunc;
        private readonly Func<T, bool> mCanExecute;
        private Action<Exception> mExceptionHandler;

        private bool mIsExecuting;
        public bool IsExecuting
        {
            get { return mIsExecuting; }
            private set
            {
                if (mIsExecuting == value)
                    return;
                mIsExecuting = value;
                OnPropertyChanged();
                OnCanExecuteChanged();
            }
        }

        #endregion

        protected RelayCommandBase2(Action<T> aExecute, Func<T, bool> aCanExecute = null, Action<Exception> aExceptionHandler = null)
        {
            if (aExecute == null)
                throw new ArgumentNullException("aExecute");

            mAction = aExecute;
            mCanExecute = aCanExecute;
            mExceptionHandler = aExceptionHandler;
        }

        protected RelayCommandBase2(Func<T, Task> aExecute, Func<T, bool> aCanExecute = null, Action<Exception> aExceptionHandler = null)
        {
            if (aExecute == null)
                throw new ArgumentNullException("aExecute");

            mFunc = aExecute;
            mCanExecute = aCanExecute;
            mExceptionHandler = aExceptionHandler;
        }

        #region ICommand
        public bool CanExecute(object aArg)
        {
            var lArg = ConvertValue(aArg);
            return !mIsExecuting && (mCanExecute == null || mCanExecute(lArg));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual async void Execute(object aArg)
        {
            IsExecuting = true;
            try
            {
                var lArg = ConvertValue(aArg);

                if (mAction != null)
                {
                    mAction(lArg);
                    return;
                }

                if (mFunc != null)
                {
                    await mFunc(lArg);
                    return;
                }
            }
            catch (Exception lEx)
            {
                //mLog.ErrorFormat("Executing command...error: {0}", Messages.BuildErrorMessage(lEx));
                if (mExceptionHandler == null)
                    throw;
                else
                    mExceptionHandler(lEx);
            }
            finally
            {
                IsExecuting = false;
            }
        }
        #endregion

        protected static T ConvertValue(object aValue)
        {
            if (!(aValue is IConvertible))
                return (T)aValue;

            if (aValue == null)
            {
                if (default(T) == null)
                {
                    return default(T);
                }
                throw new Exception(string.Format("Couldn't pass \"null\" value to variable of type \"{0}\" !", typeof(T).Name));
            }

            var lType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var lValue = (T)Convert.ChangeType(aValue, lType);
            return lValue;
        }

        protected void OnCanExecuteChanged()
        {
            var lCurrent = Application.Current;
            if (lCurrent == null)
                return;

            var lDispatcher = lCurrent.Dispatcher;
            if (lDispatcher == null || lDispatcher.HasShutdownStarted || lDispatcher.HasShutdownFinished)
                return;

            try
            {
                lDispatcher.Invoke((Action)(() => CommandManager.InvalidateRequerySuggested()));
            }
            catch { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string aPropertyName = null)
        {
            PropertyChangedEventHandler lHandler = PropertyChanged;
            if (lHandler != null) lHandler(this, new PropertyChangedEventArgs(aPropertyName));
        }

    }

    public class RelayCommand : RelayCommandBase2<object>
    {
        public RelayCommand(Action aExecute, Func<bool> aCanExecute = null, Action<Exception> aExceptionHandler = null)
            : base(a => aExecute(), aCanExecute == null ? null : new Func<object, bool>(a => aCanExecute()), aExceptionHandler)
        {
        }
    }

    public class RelayCommandAsync : RelayCommandBase2<object>
    {
        public RelayCommandAsync(Func<Task> aExecute, Func<bool> aCanExecute = null, Action<Exception> aExceptionHandler = null)
            : base(a => aExecute(), aCanExecute == null ? null : new Func<object, bool>(a => aCanExecute()), aExceptionHandler)
        { }
    }

    public class RelayCommand<T> : RelayCommandBase2<T>
    {
        public RelayCommand(Action<T> aExecute, Func<T, bool> aCanExecute = null, Action<Exception> aExceptionHandler = null)
            : base(aExecute, aCanExecute, aExceptionHandler)
        {
        }
    }

    public class RelayCommandAsync<T> : RelayCommandBase2<T>
    {
        public RelayCommandAsync(Func<T, Task> aExecute, Func<T, bool> aCanExecute = null)
            : base(aExecute, aCanExecute)
        {
        }
    }


    public abstract class RelayCommandBase<TExecute, TCanExecute> : ICommand
        where TCanExecute : class
    {
        #region Fields & Properties
        protected readonly TExecute mExecute;
        protected readonly TCanExecute mCanExecute;

        private bool mIsExecuting;

        public bool IsExecuting
        {
            get { return this.mIsExecuting; }
            protected set
            {
                if (this.mIsExecuting == value)
                    return;
                this.mIsExecuting = value;
                this.OnCanExecuteChanged();
            }
        }

        public RelayCommandBase()
        { }
        #endregion

        public RelayCommandBase(TExecute aExecute) : this(aExecute, (TCanExecute)null)
        { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="aExecute">The execution logic.</param>
        /// <param name="aCanExecute">The execution status logic.</param>
        public RelayCommandBase(TExecute aExecute, TCanExecute aCanExecute)
        {
            if (aExecute == null)
                throw new ArgumentNullException("aExecute");

            mExecute = aExecute;
            mCanExecute = aCanExecute;
        }

        #region ICommand
        //[DebuggerStepThrough]
        public bool CanExecute(object aArg)
        {
            return !mIsExecuting && (mCanExecute == null || CanExecuteInternal(aArg));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual void Execute(object aArg)
        {
            IsExecuting = true;
            try
            {
                ExecuteInternal(aArg);
            }
            finally
            {
                IsExecuting = false;
            }
        }
        #endregion 

        protected T ConvertValue<T>(object aValue)
        {
            if (!(aValue is IConvertible))
                return (T)aValue;

            if (aValue == null)
            {
                if (default(T) == null)
                {
                    return default(T);
                }
                throw new Exception(string.Format("Couldn't pass \"null\" value to variable of type \"{0}\" !", typeof(T).Name));
            }

            var lType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            var lValue = (T)Convert.ChangeType(aValue, lType);
            return lValue;
        }

        protected void OnCanExecuteChanged()
        {
            var lCurrent = Application.Current;
            if (lCurrent == null) return;
            var lDispatcher = lCurrent.Dispatcher;
            if (lDispatcher == null) return;
            lDispatcher.Invoke((Action)(() => CommandManager.InvalidateRequerySuggested()));
        }


        protected abstract void ExecuteInternal(object aArg);

        protected abstract bool CanExecuteInternal(object aArg);
    }


    public class RelayAsyncCommandEventArgs
    {
        public Exception Exception { get; private set; }

        public bool IsCanceled { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsFaulted { get; private set; }

        public RelayAsyncCommandEventArgs(Exception aExcetpion, bool aIsCanceled, bool aIsCompleted, bool aIsFaulted)
        {
            this.Exception = aExcetpion;
            this.IsCanceled = aIsCanceled;
            this.IsCompleted = aIsCompleted;
            this.IsFaulted = aIsFaulted;
        }
    }

    public class RelayAsyncCommandEventArgs<T> : RelayAsyncCommandEventArgs
    {
        public T Parameter { get; set; }

        public RelayAsyncCommandEventArgs(Exception aExcetpion, bool aIsCanceled, bool aIsCompleted, bool aIsFaulted, T aParameter)
            : base(aExcetpion, aIsCanceled, aIsCompleted, aIsFaulted)
        {
            this.Parameter = aParameter;
        }
    }

    public abstract class RelayAsyncCommandBase<TExecute, TCanExecute> : RelayCommandBase<TExecute, TCanExecute>
        where TCanExecute : class
    {
        private readonly Action<RelayAsyncCommandEventArgs> mExecuteAfter;
        private CancellationTokenSource mCancellationToken;
        private Task mTask;
        private Thread mExecutingThread;

        public RelayAsyncCommandBase(TExecute aExecute, TCanExecute aCanExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute, aCanExecute)
        {
            this.mExecuteAfter = aExecuteAfter;
        }

        public RelayAsyncCommandBase(TExecute aExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute)
        {
            this.mExecuteAfter = aExecuteAfter;
        }

        #region ICommand
        public override void Execute(object aArg)
        {
            this.IsExecuting = true;
            this.mCancellationToken = new CancellationTokenSource();
            try
            {
                this.mTask = Task.Factory
                    .StartNew(this.ExecuteAsyncInternal, this.mCancellationToken.Token)
                    .ContinueWith(this.ExecuteAsyncAfterInternal);
            }
            catch (Exception ex)
            {
                this.ExecuteAfterInternal(new RelayAsyncCommandEventArgs(ex, false, false, true));
            }
        }
        #endregion 

        private void ExecuteAsyncInternal(object aArg)
        {
            mExecutingThread = Thread.CurrentThread;
            mCancellationToken.Token.Register(new Action(this.CancelInternal));
            this.ExecuteInternal(aArg);
        }

        protected void ExecuteAsyncAfterInternal(Task lTask)
        {
            ExecuteAfterInternal(new RelayAsyncCommandEventArgs((Exception)lTask.Exception, lTask.IsCanceled, lTask.IsCompleted, lTask.IsFaulted));
        }

        protected void ExecuteAfterInternal(RelayAsyncCommandEventArgs aAsyncArgs)
        {
            try
            {
                if (mExecuteAfter != null)
                    mExecuteAfter(aAsyncArgs);
            }
            finally
            {
                mExecutingThread = (Thread)null;
                mTask = (Task)null;
                this.IsExecuting = false;
            }
        }

        public void Cancel()
        {
            this.mCancellationToken.Cancel();
        }

        private void CancelInternal()
        {
            if (!this.IsExecuting || mExecutingThread == null)
                return;
            this.mExecutingThread.Abort();
        }
    }

    public class RelayAsyncCommand : RelayAsyncCommandBase<Action, Func<bool>>
    {
        public RelayAsyncCommand(Action aExecute, Func<bool> aCanExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute, aCanExecute, aExecuteAfter)
        { }

        public RelayAsyncCommand(Action aExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute, aExecuteAfter)
        { }

        protected override void ExecuteInternal(object aArg)
        {
            this.mExecute();
        }

        protected override bool CanExecuteInternal(object aArg)
        {
            return mCanExecute();
        }
    }

    public class RelayAsyncCommand<T> : RelayAsyncCommandBase<Action<T>, Func<T, Boolean>>, ICommand<T>
    {
        public RelayAsyncCommand(Action<T> aExecute, Func<T, bool> aCanExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute, aCanExecute, aExecuteAfter)
        { }

        public RelayAsyncCommand(Action<T> aExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
            : base(aExecute, aExecuteAfter)
        { }

        protected override void ExecuteInternal(object aArg)
        {
            mExecute(ConvertValue<T>(aArg));
        }

        protected override bool CanExecuteInternal(object aArg)
        {
            return mCanExecute(ConvertValue<T>(aArg));
        }
    }



    //public class RelayAsyncCommand : RelayCommand
    //{
    //    private readonly Action mExecuteBefore;
    //    private readonly Action<RelayAsyncCommandEventArgs> mExecuteAfter;
    //    private CancellationTokenSource mCancellationToken;
    //    private Task mTask;
    //    private Thread mExecutingThread;

    //    public RelayAsyncCommand(Action aExecute, Func<bool> aCanExecute, Action aExecuteBefore, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute, aCanExecute)
    //    {
    //        this.mExecuteBefore = aExecuteBefore;
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action aExecute, Func<bool> aCanExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute, aCanExecute)
    //    {
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action aExecute, Action aExecuteBefore, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute)
    //    {
    //        this.mExecuteBefore = aExecuteBefore;
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action aExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute)
    //    {
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public override bool CanExecute(object parameter)
    //    {
    //        return base.CanExecute(parameter) && !this.IsExecuting;
    //    }

    //    public void Cancel()
    //    {
    //        this.mCancellationToken.Cancel();
    //    }

    //    private void CancelInternal()
    //    {
    //        if (!this.mIsExecuting || this.mExecutingThread == null)
    //            return;
    //        this.mExecutingThread.Abort();
    //    }

    //    private void ExecuteInternal()
    //    {
    //        this.mExecutingThread = Thread.CurrentThread;
    //        this.mCancellationToken.Token.Register(new Action(this.CancelInternal));
    //        this.mExecute();
    //    }

    //    private void ExecuteAfterInternal(Task lTask)
    //    {
    //        try
    //        {
    //            if (this.mExecuteAfter == null)
    //                return;
    //            this.mExecuteAfter(new RelayAsyncCommandEventArgs((Exception)lTask.Exception, lTask.IsCanceled, lTask.IsCompleted, lTask.IsFaulted));
    //        }
    //        finally
    //        {
    //            this.mExecutingThread = (Thread)null;
    //            this.mTask = (Task)null;
    //            this.IsExecuting = false;
    //        }
    //    }

    //    public override void Execute(object parameter)
    //    {
    //        this.IsExecuting = true;
    //        this.mCancellationToken = new CancellationTokenSource();
    //        try
    //        {
    //            if (this.mExecuteBefore != null)
    //                this.mExecuteBefore();
    //            this.mTask = Task.Factory.StartNew(new Action(this.ExecuteInternal), this.mCancellationToken.Token).ContinueWith(new Action<Task>(this.ExecuteAfterInternal));
    //        }
    //        catch (Exception ex)
    //        {
    //            this.mExecuteAfter(new RelayAsyncCommandEventArgs(ex, false, false, true));
    //        }
    //    }
    //}

    //public class RelayAsyncCommand<T> : RelayCommand<T>
    //{
    //    private readonly Action<T> mExecuteBefore;
    //    private readonly Action<RelayAsyncCommandEventArgs> mExecuteAfter;
    //    private CancellationTokenSource mCancellationToken;

    //    public bool IsExecuting { get; private set; }

    //    public RelayAsyncCommand(Action<T> aExecute, Predicate<T> aCanExecute, Action<T> aExecuteBefore, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute, aCanExecute)
    //    {
    //        this.mExecuteBefore = aExecuteBefore;
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action<T> aExecute, Predicate<T> aCanExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute, aCanExecute)
    //    {
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action<T> aExecute, Action<T> aExecuteBefore, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute)
    //    {
    //        this.mExecuteBefore = aExecuteBefore;
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public RelayAsyncCommand(Action<T> aExecute, Action<RelayAsyncCommandEventArgs> aExecuteAfter)
    //        : base(aExecute)
    //    {
    //        this.mExecuteAfter = aExecuteAfter;
    //    }

    //    public override bool CanExecute(object parameter)
    //    {
    //        return base.CanExecute(parameter) && !this.IsExecuting;
    //    }

    //    public void Cancel()
    //    {
    //        this.mCancellationToken.Cancel();
    //    }

    //    public override void Execute(object parameter)
    //    {
    //        try
    //        {
    //            T lParameter = this.ConvertValue(parameter);
    //            this.IsExecuting = true;
    //            if (this.mExecuteBefore != null)
    //                this.mExecuteBefore(lParameter);
    //            Task.Factory.StartNew((Action)(() => this.mExecute(lParameter)), this.mCancellationToken.Token).ContinueWith((Action<Task>)(t =>
    //            {
    //                this.mExecuteAfter((RelayAsyncCommandEventArgs)new RelayAsyncCommandEventArgs<T>((Exception)t.Exception, t.IsCanceled, t.IsCompleted, t.IsFaulted, lParameter));
    //                this.IsExecuting = false;
    //            }));
    //        }
    //        catch (Exception ex)
    //        {
    //            this.mExecuteAfter(new RelayAsyncCommandEventArgs(ex, false, false, true));
    //        }
    //    }
    //}


    ////////////////////////////////////////////////////////////////////////////////////////

    public interface IAsyncCommand<T> : ICommand, ICommand<T>
    {
        Task<T> ExecuteAsync(object parameter);
    }

    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            TaskCompletion = WatchTaskAsync(task);
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }

        public Task<TResult> Task { get; private set; }
        public Task TaskCompletion { get; private set; }

        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion)
                    ? Task.Result
                    : default(TResult);
            }
        }

        public TaskStatus Status
        {
            get { return Task.Status; }
        }

        public bool IsCompleted
        {
            get { return Task.IsCompleted; }
        }

        public bool IsNotCompleted
        {
            get { return !Task.IsCompleted; }
        }

        public bool IsSuccessfullyCompleted
        {
            get
            {
                return Task.Status ==
                       TaskStatus.RanToCompletion;
            }
        }

        public bool IsCanceled
        {
            get { return Task.IsCanceled; }
        }

        public bool IsFaulted
        {
            get { return Task.IsFaulted; }
        }

        public AggregateException Exception
        {
            get { return Task.Exception; }
        }

        public Exception InnerException
        {
            get
            {
                return (Exception == null)
                    ? null
                    : Exception.InnerException;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return (InnerException == null)
                    ? null
                    : InnerException.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public abstract class AsyncCommandBase<T> : IAsyncCommand<T>
    {
        public abstract bool CanExecute(object parameter);

        public abstract Task<T> ExecuteAsync(object parameter);

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class AsyncCommand<TResult> : AsyncCommandBase<TResult>, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, Task<TResult>> _command;
        private readonly CancelAsyncCommand _cancelCommand;
        private NotifyTaskCompletion<TResult> _execution;

        public AsyncCommand(Func<CancellationToken, Task<TResult>> command)
        {
            _command = command;
            _cancelCommand = new CancelAsyncCommand();
        }

        public override bool CanExecute(object parameter)
        {
            return Execution == null || Execution.IsCompleted;
        }

        public override async Task<TResult> ExecuteAsync(object parameter)
        {
            CancelCommand1.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(Command(CancelCommand1.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            CancelCommand1.NotifyCommandFinished();
            RaiseCanExecuteChanged();
            return Execution.Result;
        }

        public ICommand CancelCommand
        {
            get { return CancelCommand1; }
        }

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return Execution1; }
            private set
            {
                Execution1 = value;
                OnPropertyChanged();
            }
        }

        public Func<CancellationToken, Task<TResult>> Command
        {
            get { return _command; }
        }

        private CancelAsyncCommand CancelCommand1
        {
            get { return _cancelCommand; }
        }

        public NotifyTaskCompletion<TResult> Execution1
        {
            get { return _execution; }

            set { _execution = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;

            public CancellationToken Token
            {
                get { return Cts.Token; }
            }

            public CancellationTokenSource Cts
            {
                get { return _cts; }

                set { _cts = value; }
            }

            public bool CommandExecuting
            {
                get { return _commandExecuting; }

                set { _commandExecuting = value; }
            }

            public void NotifyCommandStarting()
            {
                CommandExecuting = true;
                if (!Cts.IsCancellationRequested)
                    return;
                Cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                CommandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return CommandExecuting && !Cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                Cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    public static class AsyncCommand
    {
        public static AsyncCommand<object> Create(Func<Task> command)
        {
            return new AsyncCommand<object>(async _ =>
            {
                await command();
                return null;
            });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(_ => command());
        }

        public static AsyncCommand<object> Create(Func<CancellationToken, Task> command)
        {
            return new AsyncCommand<object>(async token =>
            {
                await command(token);
                return null;
            });
        }

        public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command)
        {
            return new AsyncCommand<TResult>(command);
        }
    }

    public sealed class DelegateCommand : ICommand
    {
        private readonly Action _command;

        public Action Command
        {
            get { return _command; }
        }

        public DelegateCommand(Action command)
        {
            _command = command;
        }

        public void Execute(object parameter)
        {
            Command();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
