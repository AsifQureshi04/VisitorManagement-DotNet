using System.ComponentModel.DataAnnotations;

namespace MFL_VisitorManagement.Dtos
{
    public class QrCodeData
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string WhomToMeet { get; set; }
        public string Department { get; set; }
        public string IdProof { get; set; }
        public string IdProofNumber { get; set; }
        public string ReasonToMeet { get; set; }
        public DateTime VisitDate { get; set; }
        public TimeOnly InTime { get; set; }
    }
}
