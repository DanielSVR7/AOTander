using AOTander.Infrastructure.Commands.Base;
using System.Windows;

namespace AOTander.Infrastructure.Commands
{
    class CloseApplicationCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
