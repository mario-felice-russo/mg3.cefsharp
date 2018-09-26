using Bridges;
using CefSharp;
using Models;
using MVVM;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace mg3.contacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            cef = new CefSharpService();
            cef.ConfigureBrowser("udf");

            CurrentAddress = cef.Config.Address;
            InitializeComponent();
            OnReadDatabase();

            var bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                // MethodInterceptor = new MethodInterceptorLogger() // intercept .net methods calls from js and log it
            };

            Browser.JavascriptObjectRepository.Register("Bridge", new BridgeCommandService(), isAsync: true, options: bindingOptions);
            Browser.FrameLoadEnd += (object sender, FrameLoadEndEventArgs e) =>
            {

            };
            Browser.IsBrowserInitializedChanged += (object sender, DependencyPropertyChangedEventArgs e) =>
            {
                IsBrowserInitialized = true;
                IsDevToolShowed = cef.Config.ShowDevtools;
            };
            Browser.LoadingStateChanged += (object sender, LoadingStateChangedEventArgs args) =>
            {
                if (!args.IsLoading)
                {

                }
            };
            Browser.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
            {
                var name = e.ObjectName;

                Debug.WriteLine($"Object {e.ObjectName} was bound successfully.");
            };
        }

        private CefSharpService cef = new CefSharpService();
        private ContactRepository repository = new ContactRepository();
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        #region Properties

        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; NotifyPropertyChanged(); } }
        private bool _isBusy = false;

        public bool IsBrowserInitialized { get { return _isBrowserInitialized; } set { _isBrowserInitialized = value; NotifyPropertyChanged(); } }
        private bool _isBrowserInitialized = false;

        public ObservableCollection<Contact> ContactList { get { return _contactList; } set { _contactList = value; NotifyPropertyChanged(); } }
        private ObservableCollection<Contact> _contactList = new ObservableCollection<Contact>();

        public string SearchString { get { return _searchString; } set { _searchString = value; NotifyPropertyChanged(); } }
        private string _searchString = null;

        public string CurrentAddress { get { return _currentAddress; } set { _currentAddress = value; NotifyPropertyChanged(); } }
        private string _currentAddress = null;

        public string Address { get { return _address; } set { _address = value; NotifyPropertyChanged(); } }
        private string _address = null;

        #endregion Properties

        #region Commands

        public RelayCommand OnNewContact
        {
            get
            {
                return _onNewContact ?? (
                  _onNewContact = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            new NewContactWindow().ShowDialog();
                            OnReadDatabase();
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onNewContact = null;

        public RelayCommand OnSearch
        {
            get
            {
                return _onSearch ?? (
                  _onSearch = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            // Search by content of txSearch
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onSearch = null;

        public RelayCommand OnReload
        {
            get
            {
                return _onReload ?? (
                    _onReload = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            Browser.Reload(true);
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onReload = null;

        public RelayCommand OnDevTools
        {
            get
            {
                return _onDevTools ?? (
                    _onDevTools = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            IsDevToolShowed = !IsDevToolShowed;
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onDevTools = null;

        public RelayCommand OnExit
        {
            get
            {
                return _onExit ?? (
                    _onExit = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            Browser.CloseDevTools();
                            if (!Browser.IsDisposed) Browser.Dispose();
                            Environment.Exit(0);
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onExit = null;

        public RelayCommand OnToggleDevTools
        {
            get
            {
                return _onToggleDevTools ?? (
                    _onToggleDevTools = new RelayCommand(
                    (p) =>
                    {
                        if (!_isBusy)
                        {
                            IsBusy = true;
                            IsDevToolShowed = !IsDevToolShowed;
                            IsBusy = false;
                        }
                    },
                    (p) => !_isBusy)
                );
            }
        }
        private RelayCommand _onToggleDevTools = null;

        public bool IsDevToolShowed
        {
            get
            {
                return _isDevToolShowed;
            }
            set
            {
                _isDevToolShowed = value;

                if (IsBrowserInitialized)
                {
                    if (_isDevToolShowed)
                        Browser.ShowDevTools();
                    else
                        Browser.CloseDevTools();
                }

                NotifyPropertyChanged();
            }
        }
        private bool _isDevToolShowed = false;

        private void OnWindowClose(object sender, CancelEventArgs e)
        {
            Cef.Shutdown();
        }

        #endregion Commands

        #region Internal methods

        private void OnReadDatabase()
        {
            List<Contact> contacts = null;
            Task.Run(
                async () =>
                {
                    IsBusy = true;
                    contacts = (await repository.SelectAllAsync()).Entities;

                    if (contacts != null)
                    {
                        dispatcher.Invoke(
                            () =>
                            {
                                if (ContactList != null && ContactList.Count > 0)
                                    ContactList.Clear();
                                foreach (Contact c in contacts)
                                    ContactList.Add(c);
                            }
                        );
                    }

                    IsBusy = false;
                }
            );
        }

        public object EvaluateScript(IBrowser browser, string script, TimeSpan timeout)
        {
            object result = null;

            if (!browser.IsLoading && !Browser.IsDisposed)
            {
                var task = Browser.EvaluateScriptAsync(script, timeout);
                var complete = task.ContinueWith(
                    t =>
                    {
                        if (!t.IsFaulted)
                        {
                            var response = t.Result;
                            result = response.Success ? (response.Result ?? "null") : response.Message;
                        }
                    },
                    TaskScheduler.Default
                );
                complete.Wait();
            }

            return result;
        }

        public void ExceuteJavascript(string jsScript)
        {
            Browser.EvaluateScriptAsync(jsScript).ContinueWith(
                x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        Debug.WriteLine("result -> {0}", response.Result.ToString());
                    }
                }
            );
        }

        #endregion Internal methods
    }
}
