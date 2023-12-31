﻿using Steam.Messages;
using Steam.Models;
using Steam.Service;
using Steam.Service.Base;
using Steam.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Steam.ViewModel;

public class AddBalanceVM : ViewModelBase
{
    private string howMuch;
    public string HowMuch
    {
        get => howMuch;
        set => base.PropertyChange(out howMuch, value);
    }

    private User currentUser { get; set; }

    private Command apply;
    private IMessenger messenger;

    public Command Apply
    {
        get => new Command(() => ApplyCommand());
        set => base.PropertyChange(out apply, value);
    }

    private void ApplyCommand()
    {
        var query = App.ServiceContainer.GetInstance<EntityFramework>().Cards.Where(x => x.Id == currentUser.CardId).ToList();
        currentUser.Card = query.First();
        if(HowMuch.Any(x => char.IsLetter(x)))
        {
            MessageBox.Show("Error,Money is char!");
            return;
        }
        if(double.Parse(HowMuch) >= 2_000_000)
        {
            MessageBox.Show("Error,Money is too big!");
            return;
        }
        if(currentUser.Card.CardNumber == null)
        {
            Console.WriteLine();
            MessageBox.Show("Error,No card");
            return;
        }
        currentUser.Card.Balance += double.Parse(HowMuch);
        MessageBox.Show("All done");
        App.ServiceContainer.GetInstance<EntityFramework>().SaveChanges();
        App.ServiceContainer.GetInstance<MainVM>().Tosettings.Execute(null);
    }



    public AddBalanceVM(IMessenger messenger)
    {
        this.messenger = messenger;
        messenger.Subscribe<GetCurrentUser>((message) =>
        {
            if (message is GetCurrentUser user)
            {
                currentUser = user.User;
            }
        });

    }
}
