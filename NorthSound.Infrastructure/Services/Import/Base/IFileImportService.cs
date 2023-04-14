using NorthSound.Domain.Models;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IFileImportService
{
    SongFile? ExecuteImport();
    IEnumerable<SongFile> GetImportedCollection();
    SongFile Import(SongFile song);
}