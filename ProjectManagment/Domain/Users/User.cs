using Domain.ProjectUsers;
using Domain.Roles;
using Domain.Tasks;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    
    public string Name { get; private set; }
    public string Email { get; private set; }
    
    public RoleId RoleId { get;  }
    public Role? Role { get; }

    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    
    public ProjectTaskId? ProjectTaskId { get; }
    public ProjectTask? ProjectTask { get; }
}