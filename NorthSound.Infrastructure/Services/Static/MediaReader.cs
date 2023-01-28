using NorthSound.Domain.Models;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Static;

public static class MediaReader
{
    // Конвертирует метаданные файла в модель Song
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
            Path = new Uri(songPath, UriKind.Absolute),
        };
    }

    // Конвертирует title файла в модель Song
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
            string formatedName = Regex.Replace(songInfo.Name, @"\.(\w)+", "", RegexOptions.Compiled);
            string[] splittedName = Regex.Split(formatedName, @" - ", RegexOptions.Compiled);

            songTemplate.Author = splittedName[0];
            songTemplate.Name = string.Join(' ', splittedName[1..]);
        }
        catch (Exception)
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

