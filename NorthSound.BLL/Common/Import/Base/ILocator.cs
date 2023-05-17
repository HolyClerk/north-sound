using NorthSound.Domain.Models;

namespace NorthSound.BLL.Services.Import.Base;

public interface ILocator
{
    void Locate(SongFile song);
}