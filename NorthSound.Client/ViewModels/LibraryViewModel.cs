using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Services.Base;
using System.Collections.ObjectModel;

namespace NorthSound.Client.ViewModels;

internal sealed class LibraryViewModel : ViewModelBase
{
    private IRepository<Song> _repository;

    private ObservableCollection<Song> _localAudioCollection;

    private string _filter;

    public LibraryViewModel(IRepository<Song> repository)
    {
        _repository = repository;
        _localAudioCollection = new()
        {
            new Song()
            {
                Name = "FFF",
                Author = "NONE"
            }
        };
    }

    public ObservableCollection<Song> LocalAudioCollection
    {
        get => _localAudioCollection;
        set
        {
            Set(ref _localAudioCollection, value);
            _repository.ReplaceCollection(_localAudioCollection);
        }
    }

    public string Filter
    {
        get => _filter;
        set
        {
            Set(ref _filter, value);
        }
    }
}
