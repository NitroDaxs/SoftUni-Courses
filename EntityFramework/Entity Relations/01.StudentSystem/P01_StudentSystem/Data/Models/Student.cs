using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.NameMaxLength)]
        [Unicode(true)]
        public string Name { get; set; } = null!;

        [StringLength(ValidationConstants.ExactPhoneNumberLength)]
        [Unicode(false)]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

    }
}
