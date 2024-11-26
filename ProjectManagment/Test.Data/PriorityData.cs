using Domain.Priorities;

namespace Test.Data;

public class PriorityData
{
    public static ProjectPriority MainPriority()
        => ProjectPriority.New(ProjectPriorityId.New(), "TestPriority");
    public static ProjectPriority ExtraPriority()
        => ProjectPriority.New(ProjectPriorityId.New(), "ExtraTestPriority");
}