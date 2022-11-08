using NorthSound.Domain.Models;
using System;
using System.Windows.Media;

namespace NorthSound.Client.Services;

internal class PlayerShellService
{
    private MediaPlayer _player;
    private Sound? _selectedAudioSound;

	public PlayerShellService()
	{
        _player = new MediaPlayer();
        _player.MediaOpened += (s, e) => IsPlaying = true;
        _player.MediaEnded += (s, e) => IsPlaying = false;

        IsPlaying = false;
    }

    public Sound? SelectedAudioSound
    {
        get => _selectedAudioSound;
        set
        {
            _selectedAudioSound = value;

            if (_selectedAudioSound != null)
            {
                _player.Open(new Uri(_selectedAudioSound.RelativePath, UriKind.Relative));
            }
        }
    }

    public bool IsPlaying
    {
        get;
        set;
    }

    public void SetBalance(double value)
    {
        _player.Balance = value;
    }

    public void Play() => _player.Play();

    public void Stop() => _player.Stop();
}