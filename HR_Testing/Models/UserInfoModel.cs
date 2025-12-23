

using Hr_Testing.Entities;

namespace Hr_Testing.Models
{
    public class UserInfoModel : UserInfo
    {

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string RoleName { get; set; } = null!;
    }
}
