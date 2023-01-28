using NorthSound.Infrastructure.Commands.Base;

namespace NorthSound.Infrastructure.Services.Base;

public interface ISongImporter
{
    AsyncRelayCommand AsyncImportCommand { get; }
    void InitImport();
}