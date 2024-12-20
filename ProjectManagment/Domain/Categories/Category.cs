namespace Domain.Categories;

public class Category
{
    public CategoryId Id { get; }

    public string Name { get; private set; }

    private Category(
        CategoryId id,
        string name)
    {
        Id = id;
        Name = name;
    }

    public static Category New(CategoryId categoryId, string categoryName)
        => new(categoryId, categoryName);

    public void UpdateDetails(string categoryName)
    {
        Name = categoryName;
    }
}