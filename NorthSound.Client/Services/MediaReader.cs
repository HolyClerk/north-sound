using NorthSound.Domain.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NorthSound.Client.Services;

public static class MediaReader
{
    /// <summary>
    /// Конвертирует метаданные файла в модель Song
    /// </summary>
    public static async Task<Song?> ConvertMetadataAsync(string songPath)
    {
        var metadataBuffer = new byte[128];

        using (var fileStream = new FileStream(songPath, FileMode.Open))
        {
            fileStream.Seek(-128, SeekOrigin.End);
            await fileStream.ReadAsync(metadataBuffer.AsMemory(0, 128));
            fileStream.Close();
        }

        string flag = Encoding.Default.GetString(metadataBuffer, 0, 3);

        if (flag.CompareTo("TAG") != 0)
        {
            return null;
        }

        return new Song()
        {
            Name = Encoding.Default.GetString(metadataBuffer, 3, 30),
            Author = Encoding.Default.GetString(metadataBuffer, 33, 30),
            Description = Encoding.Default.GetString(metadataBuffer, 97, 30),
            Path = new Uri(songPath, UriKind.Absolute),
        };
    }

    /// <summary>
    /// Конвертирует title файла в модель Song
    /// </summary>
    public static Song ConvertToSong(FileInfo songInfo)
    {
        var songTemplate = new Song()
        {
            Author = "None",
            Name = "None",
            Path = new Uri(songInfo.FullName, UriKind.Absolute),
        };

        try
        {
            string songName = songInfo.Name;

            var author = new string(songInfo.Name.TakeWhile(symbol => symbol != '-').ToArray());
            var name = new string(songName.SkipWhile(symbol => symbol != '-').Where(symbol => symbol != '-').ToArray());

            name = name.Remove(name.Length - 4);    // Удаление расширения из названия
            name = name.Remove(0, 1);               // Удаление пробела в начале названия

            songTemplate.Author = string.IsNullOrWhiteSpace(author) ? songTemplate.Author : author;
            songTemplate.Name = string.IsNullOrWhiteSpace(name) ? songTemplate.Name : name;
        }
        catch (Exception e)
        {
            throw;
        }
        
        return songTemplate;
    }

    public static bool TryFindMediaFile(string songPath, out FileInfo songInfo)
    {
        songInfo = new FileInfo(songPath);

        if (songInfo.Exists)
        {
            return true;
        }

        return false;
    }
}

