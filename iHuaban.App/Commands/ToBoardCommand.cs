﻿using System;
using System.Windows.Input;
using Windows.UI.Popups;

namespace iHuaban.App.Commands
{
    public class ToBoardCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await new MessageDialog("ToBoardCommand", "ToBoardCommand").ShowAsync();
        }
    }
}
