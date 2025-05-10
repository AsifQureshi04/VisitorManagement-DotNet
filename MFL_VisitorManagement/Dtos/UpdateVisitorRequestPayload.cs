namespace MFL_VisitorManagement.Dtos
{
    public class UpdateVisitorRequestPayload
    {
        public int VisitorId { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string VisitorPass { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
