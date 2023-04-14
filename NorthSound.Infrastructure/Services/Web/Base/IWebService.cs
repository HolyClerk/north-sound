using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Web.Base;

public interface IWebService
{
    Action<SongFile> Downloaded { get; set; }

    Task<IEnumerable<VirtualSong>> GetOnlineCollectionAsync();
    Task<SongFile?> DownloadAsync(VirtualSong virtualSong);

    bool IsServerOnline();
}
