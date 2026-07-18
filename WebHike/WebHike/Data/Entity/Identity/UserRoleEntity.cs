using Microsoft.AspNetCore.Identity;

namespace WebHike.Data.Entities.Identity;

public class UserRoleEntity : IdentityUserRole<int>
{
    public UserEntity User { get; set; } = null!;

    public RoleEntity Role { get; set; } = null!;
}