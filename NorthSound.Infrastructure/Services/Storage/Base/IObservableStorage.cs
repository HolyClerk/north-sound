﻿using NorthSound.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthSound.Infrastructure.Services.Storage.Base;

public interface IObservableStorage<T>
{
    ObservableCollection<T> Collection { get; }
}