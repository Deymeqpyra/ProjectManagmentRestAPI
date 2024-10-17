namespace Domain.Categories;

public class CategoryId(Guid value)
{
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId Empty() => new (Guid.Empty);

    public override string ToString() => value.ToString();
}