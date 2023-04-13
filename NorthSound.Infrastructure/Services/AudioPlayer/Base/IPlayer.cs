using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using System;

namespace NorthSound.Infrastructure.Services.AudioPlayer.Base;

public interface IPlayer
{
    event Action Ended;

    SongModel Current { get; }
    bool IsPlaying { get; }

    void Pause();
    void Play();
    void Open(SongFile? selectedSong);
    void OpenStream(VirtualSong? selectedSong);

}
