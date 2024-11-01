using Domain.Categories;
using Domain.Projects;
using Domain.Users;

namespace Domain.Tasks;

public class ProjectTask
{
    public ProjectTaskId ProjectTaskId { get; }

    public string Title { get; private set; }
    public string ShortDescription { get; private set; }

    public bool IsFinished { get; private set; }

    public ProjectId ProjectId { get; private set; }
    public Project? Project { get; }

    public CategoryId CategoryId { get; private set; }
    public Category? Category { get; }

    private ProjectTask(
        string title,
        string shortDescription,
        bool isFinished,
        ProjectId projectId,
        CategoryId categoryId)
    {
        Title = title;
        ShortDescription = shortDescription;
        IsFinished = isFinished;
        ProjectId = projectId;
        CategoryId = categoryId;
    }

    public static ProjectTask New(
        string title,
        string shortDesc,
        bool isFinished,
        ProjectId projectId,
        CategoryId categoryId)
        => new(title, shortDesc, isFinished, projectId, categoryId);

    public void UpdateDetails(
        string title,
        string shortDescription,
        CategoryId categoryId)
    {
        Title = title;
        ShortDescription = shortDescription;
        CategoryId = categoryId;
    }

    public void FinishTask( )
    {
        IsFinished = true;
    }
}