/*using System.Xml.Linq;

namespace NorthSound.Domain.Models.Garbage;

public class Song
{
    public Song()
    {
        Name = string.IsNullOrWhiteSpace(Name) ? "Неизвестно" : Name;
        Author = string.IsNullOrWhiteSpace(Author) ? "Неизвестно" : Author;
    }

    public string GeneratedTitle
    {
        get => $"{Name} - {Author}";
    }

    public string Name
    {
        get;
        set;
    }

    public string Author
    {
        get;
        set;
    }

    public string RelativePath
    {
        get;
        set;
    }

    public string? AbsolutePath
    {
        get;
        set;
    }

    public string? Description
    {
        get;
        set;
    }

    public bool IsAnyPropsContains(string filter)
    {
        return
            Name.ToLower().Contains(filter.ToLower()) ||
            Author.ToLower().Contains(filter.ToLower());
    }

    public static Song[] GetTemplateAudios()
    {
        return new Song[]
        {
            new Song() { Name = "Seven Nation Army" },
            new Song() { Name = "Sweet Dreams", Author = "Marilyn Manson" },
            new Song() { Name = "Закройте", Author = "лампабикт" },
            new Song() { Name = "Пачка сигарет", Author = "Виктор Цой - КИНО" },
            new Song() { Name = "Пачка сигарет", Author = "Lizer" },
            new Song() { Name = "ОЙДА", Author = "Oxxxymiron" },
        };
    }

    public override string ToString()
    {
        return GeneratedTitle;
    }
}*/