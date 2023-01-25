namespace NorthSound.Domain.Models;

public class Song
{
    public int Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public string Author { get; set; } = default!;
    public Uri Path { get; set; } = default!;

    public bool IsAnyPropsContains(string filter)
    {
        return
            Name.ToLower().Contains(filter.ToLower()) ||
            Author.ToLower().Contains(filter.ToLower());
    }
}
