﻿using NorthSound.BLL.Other;
using NorthSound.Domain.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace NorthSound.Infrastructure;

public class MediaReader : IMediaReader
{
    public string MusicPath => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\NorthSound\";
    public string DownloadPath => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

    // Конвертирует метаданные файла в модель Song
    public async Task<SongModel?> ConvertMetadataAsync(string songPath)
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

        return new SongFile()
        {
            Name = Encoding.Default.GetString(metadataBuffer, 3, 30),
            Author = Encoding.Default.GetString(metadataBuffer, 33, 30),
            Path = new Uri(songPath, UriKind.Absolute),
        };
    }

    // Конвертирует title файла в модель Song
    public SongFile ConvertToSong(FileInfo songInfo)
    {
        var songTemplate = new SongFile()
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

    public bool TryFindMediaFile(string songPath, out FileInfo songInfo)
    {
        songInfo = new FileInfo(songPath);

        if (songInfo.Exists)
        {
            return true;
        }

        return false;
    }
}

