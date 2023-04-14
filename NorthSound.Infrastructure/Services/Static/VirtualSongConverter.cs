using NorthSound.Domain.Models;
using System;

namespace NorthSound.Infrastructure.Services.Static;

public static class VirtualSongConverter
{
    // TODO: Убрать хардкод
    public const string DownloadApiPath = "http://localhost:5000/api/stream/";
    public const string StreamApiPath = "http://localhost:5000/api/stream/";

    public static Uri GetUriLinkToStream(VirtualSong virtualSong)
        => new(StreamApiPath + virtualSong.Id);
}
