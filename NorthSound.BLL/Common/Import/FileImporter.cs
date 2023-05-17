using Microsoft.Win32;
using NorthSound.Domain.Models;
using NorthSound.BLL.Services.Import.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NorthSound.BLL.Other;

namespace NorthSound.BLL.Services.Import;

public class FileImporter : IFileImporter
{
    private readonly IMediaReader _mediaReader;

    public FileImporter(IMediaReader mediaReader)
    {
        _mediaReader = mediaReader;
    }

    private void InitializeFolder(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);

        if (pathInfo.Exists)
            return;

        pathInfo.Create();
    }

    public SongFile? ExecuteImport()
    {
        var dialogue = new OpenFileDialog();

        if (dialogue.ShowDialog() is true)
        {
            try
            {
                var songInfo = new FileInfo(dialogue.FileName);
                songInfo.CopyTo(_mediaReader.MusicPath + songInfo.Name);

                var song = _mediaReader.ConvertToSong(songInfo);
                return song;
            }
            catch (Exception)
            {
                MessageBox.Show("Песня с таким именем уже существует!");
            }
        }

        return null;
    }

    public IEnumerable<SongFile> GetCollection()
    {
        InitializeFolder(_mediaReader.MusicPath);

        var songsCollection = new List<SongFile>();
        // Паттерн *.mp3
        string[] audiofilesPath = Directory.GetFiles(_mediaReader.MusicPath, "*.mp3");

        // Поиск аудиофайлов в директории
        foreach (string audiofile in audiofilesPath)
        {
            if (_mediaReader.TryFindMediaFile(audiofile, out var songInfo))
            {
                SongFile song = _mediaReader.ConvertToSong(songInfo);
                songsCollection.Add(song);
            }
        }

        return songsCollection;
    }

    public SongFile Add(SongFile entity)
    {
        var songInfo = new FileInfo(entity.Path!.LocalPath);
        var newPath = _mediaReader.MusicPath + songInfo.Name;

        songInfo.CopyTo(newPath, true);
        entity.Path = new Uri(newPath);
        return entity;
    }

    public bool Delete()
    {
        throw new NotImplementedException();
    }
}
