using NorthSound.Domain.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NorthSound.Client.Services;

class MediaReader
{
    /// <summary>
    /// Конвертирует метаданные файла в модель Song
    /// </summary>
    public async Task<Song?> ConvertMetadataAsync(string songPath)
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
    /// <param name="songPath"></param>
    /// <returns></returns>
    public Song ConvertTitle(string songPath)
    {
        var songInfo = new FileInfo(songPath);

        var songTemplate = new Song()
        {
            Author = "None",
            Name = "None",
            Path = new Uri(songPath, UriKind.Absolute),
        };

        if (!songInfo.Exists)
        {
            return songTemplate;
        }

        try
        {
            string songName = songInfo.Name;

            var author = new string(songInfo.Name.TakeWhile(symbol => symbol != '-').ToArray());
            var name = new string(songName.SkipWhile(symbol => symbol != '-').Where(symbol => symbol != '-').ToArray());

            name = name.Remove(name.Length - 4);

            songTemplate.Author = string.IsNullOrWhiteSpace(author) ? songTemplate.Author : author;
            songTemplate.Name = string.IsNullOrWhiteSpace(name) ? songTemplate.Name : name;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        
        return songTemplate;
    }
}

