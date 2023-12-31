﻿using Steam.Messages;
using Steam.Models;
using Steam.Service;
using Steam.Service.Base;
using Steam.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.ViewModel;

public class ProfileVm : ViewModelBase
{

    private IMessenger messenger;
    public ObservableCollection<Game> GamesLib { get; set; } = new ObservableCollection<Game>();

    private User currentUser { get; set; }

    private string avatarUrl;
    public string AvatarUrl
    {
        get => avatarUrl;
        set => base.PropertyChange(out  avatarUrl, value);
    }

    private string nickname;
    public string Nickname
    {
        get => nickname;
        set => base.PropertyChange(out nickname, value);
    }


    public ProfileVm(IMessenger messenger)
    {
        this.messenger = messenger;
        messenger.Subscribe<GetCurrentUser>((message) =>
        {
            if (message is GetCurrentUser user)
            {
                currentUser = user.User;
                AvatarUrl = currentUser.AvatarUrl;
                Nickname = currentUser.Nickname;
            }
        });

        messenger.Subscribe<UpdateLibary>((message) =>
        {
            Update();
        });

    }

    private void Update()
    {

        GamesLib.Clear();
        var libary = App.ServiceContainer.GetInstance<EntityFramework>().UserGames.ToList();
        var game = App.ServiceContainer.GetInstance<EntityFramework>().Games.ToList();
        foreach (var item in libary)
        {
            foreach (var items in game)
            {
                if (item.GameId == items.Id && item.UserId == currentUser.Id)
                {
                    GamesLib.Add(items);
                }
            }
        }
    }
}
