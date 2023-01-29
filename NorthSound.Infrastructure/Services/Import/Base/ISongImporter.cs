using NorthSound.Infrastructure.Commands.Base;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface ISongImporter
{
    AsyncRelayCommand AsyncImportCommand { get; }
    void InitializeImportedStorage();
}