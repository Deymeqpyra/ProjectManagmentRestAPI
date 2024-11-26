using Domain.Statuses;

namespace Test.Data;

public class StatusData
{
    public static ProjectStatus MainStatus()
        => ProjectStatus.New(ProjectStatusId.New(), "MainExtra");
    public static ProjectStatus ExtraStatus()
        => ProjectStatus.New(ProjectStatusId.New(), "MainExtra");
}