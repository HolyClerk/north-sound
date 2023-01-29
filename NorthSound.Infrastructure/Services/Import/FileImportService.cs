using Microsoft.Win32;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace NorthSound.Infrastructure.Services.Import;

public class FileImportService : IFileImportService
{
    public static string DefaultPath
    {
        get
        {
            var localAppPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return $@"{localAppPath}\NorthSound\";
        }
    }

    public Song? ExecuteImport()
    {
        var dialogue = new OpenFileDialog();

        if (dialogue.ShowDialog() is true)
        {
            try
            {
                var songInfo = new FileInfo(dialogue.FileName);
                songInfo.CopyTo(DefaultPath + songInfo.Name);

                var song = MediaReader.ConvertToSong(songInfo);
                return song;
            }
            catch (Exception)
            {
                MessageBox.Show("Песня с таким именем уже существует!");
            }
        }

        return null;
    }

    public IEnumerable<Song> GetImportedCollection()
    {
        InitializeFolder(DefaultPath);

        var songsCollection = new List<Song>();
        // Паттерн *.mp3
        string[] audiofilesPath = Directory.GetFiles(DefaultPath, "*.mp3");

        // Поиск аудиофайлов в директории
        foreach (string audiofile in audiofilesPath)
        {
            if (MediaReader.TryFindMediaFile(audiofile, out var songInfo))
            {
                Song song = MediaReader.ConvertToSong(songInfo);
                songsCollection.Add(song);
            }
        }

        return songsCollection;
    }

    private void InitializeFolder(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);
        if (pathInfo.Exists)
            return;

        pathInfo.Create();
    }
}
