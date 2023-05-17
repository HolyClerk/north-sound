using NorthSound.Domain.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System;

namespace NorthSound.BLL.Other;

public interface IMediaReader
{
    public string MusicPath => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\NorthSound\";
    public string DownloadPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

    Task<SongModel?> ConvertMetadataAsync(string songPath);

    SongFile ConvertToSong(FileInfo songInfo);

    bool TryFindMediaFile(string songPath, out FileInfo songInfo);
}
