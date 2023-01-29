using NorthSound.Domain.Models;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IFileImportService
{
    Song? ExecuteImport();
    IEnumerable<Song> GetImportedCollection();
}