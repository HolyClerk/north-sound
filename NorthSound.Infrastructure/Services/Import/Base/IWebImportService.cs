using NorthSound.Domain.Models;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IWebImportService
{
    Song? ExecuteImport();
    IEnumerable<Song> GetImportedCollection();
}