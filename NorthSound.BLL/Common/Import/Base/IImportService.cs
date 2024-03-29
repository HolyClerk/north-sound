﻿using NorthSound.Domain.Models;
using NorthSound.BLL.Commands.Base;
using System.Collections.ObjectModel;

namespace NorthSound.BLL.Common.Import.Base;

public interface IImportService
{
    ObservableCollection<SongFile> ImportedCollection { get; }

    void InitializeImportedStorage();
    void ExecuteImportDialogue();
    void Import(SongFile entity);
}