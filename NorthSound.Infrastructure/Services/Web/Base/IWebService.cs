using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Web.Base;

public interface IWebService
{
    AsyncRelayCommand AsyncDownloadCommand { get; }
    AsyncRelayCommand AsyncUpdateCommand { get; }

    ObservableCollection<VirtualSong> VirtualCollection { get; }

    Action<SongFile> Downloaded { get; set; }

    Task InitializeOnlineCollection();
}
