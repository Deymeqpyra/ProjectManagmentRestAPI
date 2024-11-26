using Domain.Priorities;
using Domain.Projects;
using Domain.Statuses;
using Domain.Users;

namespace Test.Data;

public class ProjectData
{
    public static Project MainProject(
        UserId userId, 
        ProjectStatusId projectStatusId,
        ProjectPriorityId projectPriorityId)
        => Project.New(
            ProjectId.New(), 
            "Test Project", 
            "Test Project Description", 
            userId, 
            projectStatusId,
            projectPriorityId);
}