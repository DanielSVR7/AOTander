using AOTander.Infrastructure.Commands;
using AOTander.Models;
using AOTander.ViewModels.Base;
using System;
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
        private string _Result;
        public string EnteredLogin { get => _EnteredLogin; set => Set(ref _EnteredLogin, value); }
        public string EnteredPassword { private get => _EnteredPassword; set => Set(ref _EnteredPassword, value); }
        public string Result { get => _Result; set => Set(ref _Result, value); }

        public AuthorizationWindowViewModel()
        {
            LoginCommand = new LambdaCommand(OnLoginCommandExecuted, CanLoginCommandExecute);
        }

        public ICommand LoginCommand { get; }
        public Action LoginAction { get; set; }
        private bool CanLoginCommandExecute(object p) => true;
        private async void OnLoginCommandExecuted(object p)
        {
            try
            {
                var user = (from log in db.Users where EnteredLogin == log.Login && EnteredPassword == log.Password select log).Single();
                LoginAction?.Invoke();
                Result = "Добро пожаловать!";
                await Task.Delay(1000);
                Result = string.Empty;
            }
            catch
            {
                Result = "Логин/пароль не верны";
                await Task.Delay(1000);
                Result = string.Empty;
            }
        }
    }
}
