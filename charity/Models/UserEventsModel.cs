using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public class UserEventsModel
{
    [Key] public int Id { get; set; }

    [Required] public UserModel? User { get; set; }

    [Required] public EventsModel? Event { get; set; }
}