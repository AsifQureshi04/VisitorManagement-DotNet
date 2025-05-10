namespace MFL_VisitorManagement.Entities;

[Table("USERAUTHENTICATION")]
public class UserAuthentication
{
    [Key]
    [Column("ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("USERID")]
    public string UserId { get; set; }

    [Required]
    [Column("PASSWORDHASH")]
    public string PasswordHash { get; set; }

    [Required]
    [Column("PASSWORDSALT")]
    public string PasswordSalt { get; set; }

    [Column("CREATEDON")]
    public DateTime? CreatedOn { get; set; }

    [Column("CREATEDBY")]
    public string? CreatedBy { get; set; }

    [Column("MODIFIEDON")]
    public DateTime? ModifiedOn { get; set; }

    [Column("MODIFIEDBY")]
    public string? ModifiedBy { get; set; }

    [Column("ISACTIVE")]
    public bool? IsActive { get; set; }

    [Column("ISDELETE")]
    public bool? IsDelete { get; set; }
}
