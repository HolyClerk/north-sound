using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Import;

public class SongImporter : ISongImporter
{
    private IFileImportService _importService;
    private IRepository<Song> _repository;

    public SongImporter(IFileImportService importService, IRepository<Song> repository)
    {
        _importService = importService;
        _repository = repository;
    }

    private AsyncRelayCommand _asyncImportCommand = null!;
    public AsyncRelayCommand AsyncImportCommand
    {
        get
        {
            return _asyncImportCommand ??= new AsyncRelayCommand(async Task () =>
            {
                var song = _importService.ExecuteImport();

                if (song is not null)
                    _repository.Add(song);
            });
        }
    }

    public void InitializeImportedStorage()
        => _repository.AddRange(_importService.GetImportedCollection());
}
