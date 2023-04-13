using NorthSound.Domain.Models;

namespace NorthSound.Infrastructure.Services.Import.Base;

public interface ILocator
{
    void Locate(SongFile song);
}