using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public class ApplicationModel
{
    [Key] public int Id { get; set; }

    [Required]
    [Display(Name = "Applicant")]
    public UserModel? User { get; set; }

    [Required]
    [Display(Name = "Reason for Support")]
    public string? ReasonForSupport { get; set; }

    [Required]
    [Display(Name = "Support Type")]
    public SupportType SupportType { get; set; }

    [Display(Name = "Support Document")] public string? SupportDocumentUrl { get; set; }

    [Required]
    [Display(Name = "Support Amount")]
    [Range(0, int.MaxValue)]
    public int SupportAmount { get; set; }

    [Display(Name = "Application Status")] public ApplicationStatus Status { get; set; }

    [Display(Name = "Submission Date")] public DateTime SubmissionDate { get; set; }

    [Display(Name = "Approval Date")] public DateTime? ApprovalDate { get; set; }
}

public enum SupportType
{
    OneTime,
    DailyGrocery
}

public enum ApplicationStatus
{
    Pending,
    Approved,
    Rejected
}