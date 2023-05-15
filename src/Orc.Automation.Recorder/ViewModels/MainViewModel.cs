namespace Orc.Automation.Recorder.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Automation;
    using Catel.MVVM;
    using Services;

    public class MainViewModel : ViewModelBase
    {
        private bool _inProcess = false;
        private AutomationElement _window
            ;

        private IntPtr _handler;

        public MainViewModel()
        {
            Start = new Command(OnStart, CanStart);
            Pause = new Command(OnPause, CanPause);
            Stop = new Command(OnStop, CanStop);

            Send = new Command(OnSend, CanSend);
            Comment = new Command(OnComment, CanComment);

            RefreshCandidatesList = new Command(OnRefreshCandidatesList);
        }

        public string Message { get; set; }
        public string CommentText { get; set; }

        public int? SelectedCandidateHandler { get; set; }
        public List<int> CandidateList { get; private set; }

        public Command Start { get; }
        public Command Pause { get; }
        public Command Stop { get; }
        public Command Send { get; }
        public Command Comment { get; }
        public Command RefreshCandidatesList { get; }

        private void OnMessageChanged()
        {
            Send?.RaiseCanExecuteChanged();
        }

        private void OnCommentTextChanged()
        {
            Comment?.RaiseCanExecuteChanged();
        }

        private void OnRefreshCandidatesList()
        {
            CandidateList = new List<int>();

            var searchClassName = AutomationHelper.GetActiveControlClassName(typeof(Controls.AutomationInformer));

            //TODO: Use Find<> function...but this is faster for now
            foreach (var element in AutomationElement.RootElement.GetChildElements()
                         .Where(x => x.Current.FrameworkId == "WPF"))
            {
                var children = element.GetChildElements().ToList();
                if (children.Any(x => x.Current.ClassName == searchClassName))
                {
                    CandidateList.Add(element.Current.NativeWindowHandle);
                }
            }
        }

        private void OnStart()
        {
            var selectedCandidateHandler = SelectedCandidateHandler;
            if (selectedCandidateHandler is null)
            {
                return;
            }

            _inProcess = true;

            _handler = new IntPtr(selectedCandidateHandler.Value);
            _window = AutomationElement.FromHandle(_handler);
            var informer = _window.Find<AutomationInformer>();
            informer.StartRecord();

            BuildTree(informer);

            RaiseCanExecute();
        }

        private bool CanStart()
        {
            return !_inProcess && SelectedCandidateHandler is not null;
        }

        private void BuildTree(AutomationInformer informer)
        {
            var service = new TreeBuilderService();
            service.Build(informer);
        }

        private void OnPause()
        {
            _inProcess = false;

            RaiseCanExecute();
        }

        private bool CanPause()
        {
            return _inProcess;
        }

        private void OnStop()
        {
            _inProcess = false;

            var selectedCandidateHandler = SelectedCandidateHandler;
            if (selectedCandidateHandler is null)
            {
                return;
            }

            var handler = new IntPtr(selectedCandidateHandler.Value);

            var window = AutomationElement.FromHandle(handler);
            var informer = window.Find<AutomationInformer>();

            informer.StopRecord();

            RaiseCanExecute();
        }

        private bool CanStop()
        {
            return _inProcess;
        }

        private void OnComment()
        {
            
        }

        private bool CanComment()
        {
            return !string.IsNullOrWhiteSpace(CommentText);
        }

        private void OnSend()
        {
            
        }

        private bool CanSend()
        {
            return !string.IsNullOrWhiteSpace(Message);
        }

        private void RaiseCanExecute()
        {
            Start.RaiseCanExecuteChanged();
            Stop.RaiseCanExecuteChanged();
            Pause.RaiseCanExecuteChanged();
        }
    }
}
