using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MFL_VisitorManagement.Entities
{
    public class User
    {
        public string FirstName  { get; set; }
        public string? LastName  { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId  { get; set; }
        public string? Password  { get; set; }
        public string? UserRole  { get; set; }
    }

    public class Users
    {
        [Key]
        [Column("USERID")]
        public string UserId { get; set; } = null!;

        [Required]
        [Column("FIRSTNAME")]
        public string FirstName { get; set; } = null!;

        [Column("LASTNAME")]
        public string? LastName { get; set; }

        [Required]
        [Column("EMAIL")]
        public string Email { get; set; } = null!;

        [Required]
        [Column("ROLEID")]
        public int RoleId { get; set; }

        [Column("CREATEDON")]
        public DateTime? CreatedOn { get; set; }

        [Column("CREATEDBY")]
        public string? CreatedBy { get; set; }

        [Column("MODIFIEDON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("MODIFIEDBY")]
        public string? ModifiedBy { get; set; }

        [Column("ISACTIVE")]
        public char? IsActive { get; set; }

        [Column("ISDELETE")]
        public char? IsDelete { get; set; }

        [Column("ID")]
        public int? Id { get; set; }
    }
}
