using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public class DonationModel
{
    [Key] public int Id { get; set; }

    [Required] [Display(Name = "Donor")] public UserModel? Donor { get; set; }
    [Display(Name = "Receiver")] public UserModel? Receiver { get; set; }
    [Required] [Display(Name = "Title")] public string? Title { get; set; }
    [Display(Name = "Donation Comment")] public string? Comment { get; set; }
    [Display(Name = "Image")] public string? Image { get; set; }
    [Required]
    [Display(Name = "Donation Date")]
    public DateTime AddDate { get; set; }
    // is occupied
    [Display(Name = "Donation Status")] public DonationStatus Status { get; set; }
    
    // review status
 
}

public enum DonationStatus
{
    Available,
    Occupied
}
