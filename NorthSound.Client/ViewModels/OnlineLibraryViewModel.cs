using NorthSound.Client.ViewModels.Base;
using NorthSound.Domain.Models;
using NorthSound.Infrastructure.Commands.Base;
using NorthSound.Infrastructure.Services.Import.Base;
using NorthSound.Infrastructure.Services.Web.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NorthSound.Client.ViewModels;

internal sealed class OnlineLibraryViewModel : ViewModelBase
{
    public OnlineLibraryViewModel()
    {
    }

    private ICollectionView _virtualCollectionView = default!;
    public ICollectionView VirtualCollectionView
    {
        get => _virtualCollectionView;
        set
        {
            if (_virtualCollectionView == value)
                return;

            Set(ref _virtualCollectionView, value);
        }
    }

    private string? _filter;
    public string? Filter
    {
        get => _filter;
        set
        {
            Set(ref _filter, value);

            VirtualCollectionView.Filter = FilterCollection;
        }
    }

    private bool FilterCollection(object entity)
    {
        if (entity is not VirtualSong song || string.IsNullOrWhiteSpace(Filter))
            return true;

        return song.IsAnyPropsContains(Filter);
    }

    public void UpdateVirtualCollection(ObservableCollection<VirtualSong> collection)
    {
        VirtualCollectionView = CollectionViewSource.GetDefaultView(collection);
    }
}