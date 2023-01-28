using System.Threading.Tasks;

namespace NorthSound.Infrastructure.Services.Base;

public interface IFileImportService
{
    void ExecuteImportAsync(string? pathToSave);
}