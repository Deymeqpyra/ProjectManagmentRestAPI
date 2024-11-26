using Domain.Categories;

namespace Test.Data;

public static class CategoryData
{
    public static Category MainCategory()
        => Category.New(CategoryId.New(), "TestCategory");
    public static Category ExtraCategory()
        => Category.New(CategoryId.New(), "TestExtraCategory");
}