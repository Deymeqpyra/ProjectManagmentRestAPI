using DevOne.Security.Cryptography.BCrypt;
using Domain.ProjectUsers;
using Domain.Roles;
using Domain.Tasks;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string Email { get; private set; }
    
    public RoleId RoleId { get; private set; }
    public Role? Role { get; }

    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    
    public ProjectTaskId? ProjectTaskId { get; private set; }
    public ProjectTask? ProjectTask { get; }

    private User(UserId id, string userName, string password, string email, RoleId roleId)
    {
        Id = id;
        UserName = userName;
        Password = password;
        Email = email;
        RoleId = roleId;
    }
    public static User RegisterNewUser(UserId id, string userName, string password, string email, RoleId roleId)
    => new(id, userName, BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(10)), email, roleId);

    public void UpdatePassword(string password)
    {
        Password = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
    } 
    public void UpdateEmail(string email)
    {
        Email = email;
    } 
    public void UpdateUserName(string userName)
    {
        UserName = userName;
    }
    public void SetNewRole(RoleId roleId)
    {
        RoleId = roleId;
    }

    public void AssignProjectTask(ProjectTaskId projectTaskId)
    {
        ProjectTaskId = projectTaskId;
    }
}