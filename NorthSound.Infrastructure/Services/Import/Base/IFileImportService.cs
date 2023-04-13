using NorthSound.Domain.Models;
using System.Collections.Generic;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface IFileImportService
{
    LocalSong? ExecuteImport();
    IEnumerable<LocalSong> GetImportedCollection();
    LocalSong Import(LocalSong song);
}