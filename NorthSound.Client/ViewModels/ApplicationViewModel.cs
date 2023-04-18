using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.DAL.Base;
using System;
using System.Collections.Specialized;
using NorthSound.BLL.Facades.Base;

namespace NorthSound.Client.ViewModels;

internal sealed class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel(
        PlayerViewModel songVm,
        LibraryCollectionViewModel libraryVm,
        OnlineLibraryViewModel onlineLibraryVm)
    {
        Current = this;

        PlayerVm = songVm;
        LibraryVm = libraryVm;
        OnlineLibraryVm = onlineLibraryVm;
    }

    public ApplicationViewModel Current { get; }

    public PlayerViewModel PlayerVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }
    public OnlineLibraryViewModel OnlineLibraryVm { get; }
}