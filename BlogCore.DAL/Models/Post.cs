namespace BlogCore.DAL.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class Post
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    public IList<Comment> Comments { get; } = new List<Comment>();
}
