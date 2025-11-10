namespace SocPass.Domain.DTO
{
    public class MemberCreateRequest
    {
        public int FlatId { get; set; }
        public int NumberOfAdults { get; set; }
        public List<int> ChildAges { get; set; } = new List<int>();
    }
}
 