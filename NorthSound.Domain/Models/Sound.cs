namespace NorthSound.Domain.Models;

public class Sound
{
	public Sound() { }

	public string GeneratedTitle
	{
		get
        {
			var name = string.IsNullOrWhiteSpace(Name) ? "Неизвестно" : Name;
			var author = string.IsNullOrWhiteSpace(Author) ? "Неизвестно" : Author;

			return $"{name} - {author}";
        }
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

	public string? Description
	{
		get;
		set;
	}

	public string? RelativePath
	{
		get;
		set;
	}

	public string? AbsolutePath
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

    public static Sound[] GetTemplateAudios()
    {
        return new Sound[]
        {
            new Sound() { Name = "Seven Nation Army", Author = "" },
            new Sound() { Name = "Sweet Dreams", Author = "Marilyn Manson" },
            new Sound() { Name = "Закройте", Author = "лампабикт" },
            new Sound() { Name = "Пачка сигарет", Author = "Виктор Цой - КИНО" },
            new Sound() { Name = "Пачка сигарет", Author = "Lizer" },
            new Sound() { Name = "ОЙДА", Author = "Oxxxymiron" },
        };
    }

	public override string ToString()
	{
		return GeneratedTitle;
	}
}