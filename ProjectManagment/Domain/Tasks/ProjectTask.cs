using Domain.Categories;
using Domain.Projects;
using Domain.Users;

namespace Domain.Tasks;

public class ProjectTask
{
    public ProjectTaskId TaskId { get; }

    public string Title { get; private set; }
    public string ShortDescription { get; private set; }

    public bool IsFinished { get; private set; }

    public ProjectId ProjectId { get; private set; }
    public Project? Project { get; }

    public UserId UserId { get; private set; }
    public User? User { get; }

    public CategoryId CategoryId { get; private set; }
    public Category? Category { get; }

    private ProjectTask(
        ProjectTaskId taskId,
        string title,
        string shortDescription,
        bool isFinished,
        UserId userId,
        ProjectId projectId,
        CategoryId categoryId)
    {
        TaskId = taskId;
        Title = title;
        ShortDescription = shortDescription;
        IsFinished = isFinished;
        UserId = userId;
        ProjectId = projectId;
        CategoryId = categoryId;
    }

    public static ProjectTask New(
        ProjectTaskId taskId,
        string title,
        string shortDesc,
        bool isFinished,
        UserId userId,
        ProjectId projectId,
        CategoryId categoryId)
        => new(taskId, title, shortDesc, isFinished, userId, projectId, categoryId);

    public void UpdateDetails(
        string title,
        string shortDescription,
        CategoryId categoryId)
    {
        Title = title;
        ShortDescription = shortDescription;
        CategoryId = categoryId;
    }

    public void FinishTask()
    {
        IsFinished = true;
    }
}