using System;
using System.ComponentModel.DataAnnotations;

namespace VacationManager.Models
{
    /// <summary>
    /// Class TimeOff.
    /// </summary>
    public class TimeOff
    {
        /// <summary>
        /// Gets or sets the time off identifier.
        /// </summary>
        /// <value>The time off identifier.</value>
        [Key]
        public int TimeOffId { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        [Required(ErrorMessage = "*This field is Required!")]
        [CustomDateRange(ErrorMessage = "Date cannot be less than today and more than 1 year from now.")]
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        [Required(ErrorMessage = "*This field is Required!")]
        [CustomDateRange(ErrorMessage = "Date cannot be less than today and more than 1 year from now.")]
        [DateGreaterThan("From", ErrorMessage = "Date cannot be less than from date.")]
        public DateTime To { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [half day].
        /// </summary>
        /// <value><c>true</c> if [half day]; otherwise, <c>false</c>.</value>
        [Required(ErrorMessage = "*This field is Required!")]
        public virtual bool HalfDay { get; set; } = false;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [Required(ErrorMessage = "*This field is Required!")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TimeOff"/> is approved.
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        public bool Approved { get; set; } = false;

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the requesting user.
        /// </summary>
        /// <value>The requesting user.</value>
        public string RequestingUser { get; set; }
    }
}
