using AOTander.Infrastructure.Commands;
using AOTander.Models;
using AOTander.ViewModels.Base;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AOTander.ViewModels
{
    class AuthorizationWindowViewModel : ViewModel
    {
        public TanderDatabaseEntities db = new TanderDatabaseEntities();
        private string _EnteredLogin;
        private string _EnteredPassword;
        private string _Message;
        private int _AttemptsCount = 0;
        private Captcha _Security;
        private bool _IsSecurityVisible;
        private string  _SecurityText = string.Empty;
        public string EnteredLogin { get => _EnteredLogin; set => Set(ref _EnteredLogin, value); }
        public string EnteredPassword { private get => _EnteredPassword; set => Set(ref _EnteredPassword, value); }
        public string Message { get => _Message; set => Set(ref _Message, value); }
        public Captcha Security { get => _Security; set => Set(ref _Security, value); }
        public bool IsSecurityVisible { get => _IsSecurityVisible; set => Set(ref _IsSecurityVisible, value); }
        public string SecurityText { get => _SecurityText; set => Set(ref _SecurityText, value); }
        public Users AuthorizedUser { get; set; }


        public AuthorizationWindowViewModel()
        {
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
            RefreshSecurityCommand = new LambdaCommand(OnRefreshSecurityCommandExecuted, CanRefreshSecurityCommandyExecute);
        }

        public ICommand RefreshSecurityCommand { get; }
        private bool CanRefreshSecurityCommandyExecute(object p) => IsSecurityVisible;
        private void OnRefreshSecurityCommandExecuted(object p)
        {
            ShowCaptcha();
        }
        public ICommand LoginCommand { get; }
        public Action LoginAction { get; set; }
        private bool CanLoginCommandExecute(object p) => true;
        private async void OnLoginCommandExecuted(object p)
        {
            try
            {
                if (IsSecurityVisible)
                {
                    if (SecurityText == string.Empty)
                    {
                        Message = "Введите текст в окно капчи";
                        await Task.Delay(1000);
                        Message = string.Empty;
                        return;
                    }
                    if (SecurityText != Security.Text)
                    {
                        Message = "Проверка не пройдена";
                        await Task.Delay(1000);
                        Message = string.Empty;
                        return;
                    }
                }    
                AuthorizedUser = (from log in db.Users where EnteredLogin == log.Login && EnteredPassword == log.Password select log).Single();
                LoginAction?.Invoke();
                Message = "Добро пожаловать!";
                await Task.Delay(1000);
                Message = string.Empty;
                _AttemptsCount = 0;
                IsSecurityVisible = false;
            }
            catch
            {
                Message = "Логин/пароль не верны";
                _AttemptsCount++;
                if (_AttemptsCount >= 2)
                    ShowCaptcha();
                await Task.Delay(1000);
                Message = string.Empty;
            }
        }
        private void ShowCaptcha()
        {
            Security = new Captcha(200, 60);
            IsSecurityVisible = true;
        }
    }
}
