using NorthSound.Domain.Models;

namespace NorthSound.Infrastructure.Services.Import;

public interface ILocator
{
    void Locate(LocalSong song);
}