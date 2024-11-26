using Domain.Tags;

namespace Test.Data;

public class TagData
{
    public static Tag MainTag()
        => Tag.New(TagId.New(), "TestTag");
}