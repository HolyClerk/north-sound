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
    public SongFile? ExecuteImport()
    {
        var dialogue = new OpenFileDialog();

        if (dialogue.ShowDialog() is true)
        {
            try
            {
                var songInfo = new FileInfo(dialogue.FileName);
                songInfo.CopyTo(MediaReader.MusicPath + songInfo.Name);

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

    public IEnumerable<SongFile> GetImportedCollection()
    {
        InitializeFolder(MediaReader.MusicPath);

        var songsCollection = new List<SongFile>();
        // Паттерн *.mp3
        string[] audiofilesPath = Directory.GetFiles(MediaReader.MusicPath, "*.mp3");

        // Поиск аудиофайлов в директории
        foreach (string audiofile in audiofilesPath)
        {
            if (MediaReader.TryFindMediaFile(audiofile, out var songInfo))
            {
                SongFile song = MediaReader.ConvertToSong(songInfo);
                songsCollection.Add(song);
            }
        }

        return songsCollection;
    }

    public SongFile Import(SongFile entity)
    {
        var songInfo = new FileInfo(entity.Path!.LocalPath);
        var newPath = MediaReader.MusicPath + songInfo.Name;

        songInfo.CopyTo(newPath, true);
        entity.Path = new Uri(newPath);
        return entity;
    }

    private void InitializeFolder(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);

        if (pathInfo.Exists)
            return;

        pathInfo.Create();
    }
}
