using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Domain.Models;

public class Song
{
    [Key] 
    public int Id { get; set; } = default;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = default!;

    public Uri? Path { get; set; } = default!;

    public bool IsAnyPropsContains(string filter)
    {
        return
            Name.ToLower().Contains(filter.ToLower()) ||
            Author.ToLower().Contains(filter.ToLower());
    }
}
