using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services;

public class Importer : ICommandImporter
{
    private IFileImportService _importService;

    public Importer(IFileImportService importService)
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
}
