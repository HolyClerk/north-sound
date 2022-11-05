using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class SoundViewModel : ViewModelBase
{
    private string? _selectedAudioTitle;
    private string? _filterText;
    private ICollectionView? _audioPlaylistItems;
    private Sound _selectedAudio;

    public SoundViewModel()
    {
        SelectedAudioTitle = "Test - test";

        AudioPlaylistItems = CollectionViewSource.GetDefaultView(Sound.GetTemplateAudios());
        AudioPlaylistItems.Filter = FilterAudio;
    }

    public string? FilterText
    {
        get => _filterText;
        set => Set(ref _filterText, value);
    }

    public string? SelectedAudioTitle
    {
        get => _selectedAudioTitle;
        set => Set(ref _selectedAudioTitle, value);
    }

    public ICollectionView AudioPlaylistItems
    {
        get => _audioPlaylistItems; 
        set => Set(ref _audioPlaylistItems, value);
    }

    public Sound SelectedAudio
    {
        get => _selectedAudio;
        set => Set(ref _selectedAudio, value);
    }

    private bool FilterAudio(object obj)
    {
        var currentAudio = obj as Sound;
        return string.IsNullOrWhiteSpace(FilterText)
            || currentAudio == null
            || currentAudio.IsAnyPropsContains(FilterText);
    }
}

