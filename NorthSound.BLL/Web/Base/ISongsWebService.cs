﻿using NorthSound.Domain;
using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthSound.BLL.Facades.Base;

public interface ISongsWebService
{
    Task<IEnumerable<VirtualSong>> GetOnlineCollectionAsync();
    Task<SongFile?> DownloadAsync(VirtualSong virtualSong);
}
