using Domain.Priorities;

namespace Test.Data;

public class PriorityData
{
    public static ProjectPriority MainPriority()
        => ProjectPriority.New(ProjectPriorityId.New(), "TestPriority");
}