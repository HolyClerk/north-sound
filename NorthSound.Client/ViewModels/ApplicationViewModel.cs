using NorthSound.Client.ViewModels.Base;
using NorthSound.Client.ViewModels.Auth;

namespace NorthSound.Client.ViewModels;

internal sealed class ApplicationViewModel : ViewModelBase
{
    public ApplicationViewModel(
        PlayerViewModel songVm,
        LibraryCollectionViewModel libraryVm,
        OnlineLibraryViewModel onlineLibraryVm,
        AuthenticateViewModel authenticate,
        DialoguesViewModel dialoguesVm,
        HubViewModel chatVm)
    {
        Current = this;

        PlayerVm = songVm;
        LibraryVm = libraryVm;
        OnlineLibraryVm = onlineLibraryVm;
        AuthenticateVm = authenticate;
        ChatVm = chatVm;
        DialoguesVm = dialoguesVm;
    }

    public ApplicationViewModel Current { get; }

    public PlayerViewModel PlayerVm { get; }
    public LibraryCollectionViewModel LibraryVm { get; }
    public OnlineLibraryViewModel OnlineLibraryVm { get; }
    public AuthenticateViewModel AuthenticateVm { get; }
    public HubViewModel ChatVm { get; }
    public DialoguesViewModel DialoguesVm { get; }
}