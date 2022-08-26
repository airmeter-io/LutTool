using System;
using System.Windows.Input;

namespace LutLib.View
{
    public class Command : ICommand
    {
        private readonly Action _command;

        public Command(Action pCommand) => _command = pCommand;
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _command();

        public event EventHandler? CanExecuteChanged;
    }
}