using NorthSound.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Web;

public interface IWebLibraryService : IWebConnector
{
    Task<IEnumerable<VirtualSong>> FetchVirtualSongs();
    Task<IEnumerable<VirtualSong>> FetchVirtualSongs(string pattern);
}