using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System;
using System.Collections.Specialized;
using NorthSound.BLL.Facades.Base;
using NorthSound.Client.ViewModels.Auth;

namespace NorthSound.Client.ViewModels;

internal sealed class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel(
        PlayerViewModel songVm,
        LibraryCollectionViewModel libraryVm,
        OnlineLibraryViewModel onlineLibraryVm,
        AuthenticateViewModel authenticate)
    {
        Current = this;

        PlayerVm = songVm;
        LibraryVm = libraryVm;
        OnlineLibraryVm = onlineLibraryVm;
        AuthenticateVm = authenticate;
    }

    public ApplicationViewModel Current { get; }

    public PlayerViewModel PlayerVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }
    public OnlineLibraryViewModel OnlineLibraryVm { get; }
    public AuthenticateViewModel AuthenticateVm { get; }
}