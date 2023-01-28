using Microsoft.Win32;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Static;
using System;
using System.IO;

namespace NorthSound.Infrastructure.Services;

public class FileImportService : IFileImportService
{
    private IRepository<Song> _repository;

    public FileImportService(IRepository<Song> repository)
    {
        _repository = repository;
    }

    public static string DefaultPath
    {
        get
        {
            var localAppPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return $@"{localAppPath}\NorthSound";
        }
    }

    public void ExecuteImportAsync(string? pathToSave)
    {
        var dialogue = new OpenFileDialog();

        if (dialogue.ShowDialog() is true)
        {
            var songInfo = new FileInfo(dialogue.FileName);
            var song = MediaReader.ConvertToSong(songInfo);

            _repository.Add(song);
        }
    }

    public void InitializeRepositoryStorage()
    {
        InitFolder(DefaultPath);

        // Паттерн *.mp3
        string[] audiofilesPath = Directory.GetFiles(DefaultPath, "*.mp3");

        // Поиск аудиофайлов в директории
        foreach (string audiofile in audiofilesPath)
        {
            if (MediaReader.TryFindMediaFile(audiofile, out var songInfo))
            {
                Song songTemp = MediaReader.ConvertToSong(songInfo);
                _repository.Add(songTemp);
            }
        }
    }

    private void InitFolder(string playlistsPath)
    {
        var pathInfo = new DirectoryInfo(playlistsPath);
        if (pathInfo.Exists)
            return;

        pathInfo.Create();
    }
}
