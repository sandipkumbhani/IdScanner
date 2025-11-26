namespace SocPass.Domain.DTO
{
    public class LoginUserDTO
    {
        public long UserId { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public long InsertBy { get; set; }
        public DateTime InsertDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Token { get; set; }
        public string? UserRoleName { get; set; }

    }
}
