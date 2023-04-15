using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System;

namespace NorthSound.Infrastructure.Services.AudioPlayer.Base;

public interface IPlayer
{
    event Action<bool>? PlayerStateChanged;
    event Action? SongEnded;

    SongModel Current { get; }

    void Pause();
    void Play();
    void Open(SongModel? selectedSong);
}
