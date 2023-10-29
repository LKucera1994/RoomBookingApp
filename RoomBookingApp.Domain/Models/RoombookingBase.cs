using System.ComponentModel.DataAnnotations;

namespace RoomBookingApp.Core.Models
{
    public abstract class RoombookingBase:IValidatableObject
    {
        [Required]
        [StringLength(80)]
        public string FullName { get; set; } = String.Empty;
        [Required]
        [StringLength(80)]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Date must be in the future.", new[] {nameof(Date)});
            }
        }
    }
}