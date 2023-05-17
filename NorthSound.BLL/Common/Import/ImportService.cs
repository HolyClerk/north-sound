using NorthSound.Domain.Models;
using NorthSound.BLL.Commands.Base;
using NorthSound.BLL.Services.Import.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NorthSound.BLL.Common.Import.Base;

namespace NorthSound.BLL.Common.Import;

/// <summary>
/// LocalImporter Выступает модулем/сервисом импорта.
/// 
/// Содержит в себе сервис импорта файлов из локальных кэш-папок. 
/// Модуль получает уже импортированные песни и сохраняет их в коллекцию.
/// 
/// Так же модуль имеет команду, вызывая которую поле "_fileImport" вызывает
/// необходимый функционал для пользовательского выбора импорта файла.
/// 
/// Реализацией интерфейса ILocator служит 1 метод - Locate.
/// Он необходим для того, чтобы с помощью "IFileImportService" расположить модель
/// в нужном месте (на локальном хранилище), и после добавить его в коллекцию
/// </summary>
public class ImportService : IImportService
{
    private readonly IFileImporter _fileImport;

    public ImportService(
        IFileImporter importService)
    {
        _fileImport = importService;
        ImportedCollection = new ObservableCollection<SongFile>();
    }

    public ObservableCollection<SongFile> ImportedCollection
    {
        get; private set;
    }

    public void InitializeImportedStorage()
    {
        var imported = _fileImport.GetCollection();

        foreach (var item in imported)
            ImportedCollection.Add(item);
    }

    public void ExecuteImportDialogue()
    {
        var song = _fileImport.ExecuteImport();

        if (song is not null)
            ImportedCollection.Add(song);
    }

    public void Import(SongFile entity)
    {
        var song = _fileImport.Add(entity);
        ImportedCollection.Add(song);
    }
}
