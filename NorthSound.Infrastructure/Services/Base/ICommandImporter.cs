using NorthSound.Infrastructure.Commands.Base;

namespace NorthSound.Infrastructure.Services.Base;

public interface ICommandImporter
{
    AsyncRelayCommand AsyncImportCommand { get; }
}