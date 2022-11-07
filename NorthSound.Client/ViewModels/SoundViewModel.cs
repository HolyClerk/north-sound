using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal class SoundViewModel : ViewModelBase
{
    private string _filterText = "";
    private ICollectionView? _audioPlaylistItems;
    private Sound? _selectedAudio;

    public SoundViewModel()
    {
        var soundsTemplate = new Sound[100000];

        for (int i = 0; i < soundsTemplate.Length; i++)
        {
            soundsTemplate[i] = new Sound() { Author = "FOR TEST", Name = "FOR TEST" };
        }

        AudioPlaylistItems = CollectionViewSource.GetDefaultView(soundsTemplate);
        BindFilter(FilterAudio);
    }

    public string FilterText
    {
        get => _filterText;
        set 
        {
            Set(ref _filterText, value);
            BindFilter(FilterAudio);
        }
    }

    public ICollectionView? AudioPlaylistItems
    {
        get => _audioPlaylistItems;
        set => Set(ref _audioPlaylistItems, value);
    }

    public Sound? SelectedAudio
    {
        get => _selectedAudio;
        set => Set(ref _selectedAudio, value);  
    }

    private void BindFilter(Func<object, bool> filterAudio)
    {
        if (AudioPlaylistItems == null)
        {
            return;
        }

        AudioPlaylistItems.Filter = FilterAudio;
    }

    private bool FilterAudio(object obj)
    {
        var currentAudio = obj as Sound;
        return string.IsNullOrWhiteSpace(FilterText)
            || currentAudio == null
            || currentAudio.IsAnyPropsContains(FilterText);
    }
}

