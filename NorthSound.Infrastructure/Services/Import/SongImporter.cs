using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Import;

public class SongImporter : ISongImporter
{
    private IFileImportService _importService;

    public SongImporter(IFileImportService importService)
    {
        _importService = importService;
    }

    private AsyncRelayCommand _asyncImportCommand = null!;
    public AsyncRelayCommand AsyncImportCommand
    {
        get
        {
            return _asyncImportCommand ??= new AsyncRelayCommand(async Task () =>
            {
                _importService.ExecuteImportAsync(null);
            });
        }
    }

    public void InitImport()
        => _importService.InitializeRepositoryStorage();
}
