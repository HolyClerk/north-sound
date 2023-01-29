using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IFileImportService
{
    void ExecuteImportAsync(string? pathToSave);
    void InitializeRepositoryStorage();
}