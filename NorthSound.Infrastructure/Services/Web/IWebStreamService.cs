using NorthSound.Domain.Models;
using System.IO;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Web;

public interface IWebStreamService : IWebConnector
{
    Task<LocalSong?> DownloadSong(VirtualSong song);
    Task<Stream> GetSongStream(VirtualSong song);
}

