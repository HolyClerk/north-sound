using Microsoft.Win32;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Static;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace NorthSound.Infrastructure.Services;

public class FileImportService : IFileImportService
{
    private IRepository<Song> _repository;

    public FileImportService(IRepository<Song> repository)
    {
        _repository = repository;
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
}
