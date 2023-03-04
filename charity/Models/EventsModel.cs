using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public class EventsModel
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(100)] public string? EventName { get; set; }

    [Required] [MaxLength(500)] public string? Description { get; set; }

    [Required] public string? Venue { get; set; }

    [Required] public DateTime EventDate { get; set; }

    public string? ImageUrl { get; set; }
}