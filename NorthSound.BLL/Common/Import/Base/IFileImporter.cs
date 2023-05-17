using NorthSound.Domain.Models;
using System.Collections.Generic;

namespace NorthSound.BLL.Services.Import.Base;

public interface IFileImporter
{
    SongFile? ExecuteImport();
    IEnumerable<SongFile> GetCollection();
    SongFile Add(SongFile song);
    bool Delete();
}