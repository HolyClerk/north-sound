using NorthSound.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NorthSound.BLL.Other;

public interface IRemoteSongRepository
{
    Task<IEnumerable<VirtualSong>> GetVirtualCollection();
    Task<IEnumerable<VirtualSong>> GetVirtualCollectionByPattern(string pattern);

    Task<SongFile?> GetSongFileByEntity(VirtualSong song);
    Task<Stream?> GetSongStreamByEntity(VirtualSong song);

    Task<bool> UpdateSong(SongFile song);
    Task<bool> AddSong(SongFile song);
}