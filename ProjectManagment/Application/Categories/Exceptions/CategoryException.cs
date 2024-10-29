using Domain.Categories;

namespace Application.Categories.Exceptions;

public abstract class CategoryException(CategoryId categoryId, string message, Exception? innerException = null)
        : Exception(message, innerException)
{
    public CategoryId CategoryId { get; } = categoryId;
}

public class CategoryNotFoundException(CategoryId categoryId) 
    : CategoryException(categoryId, "Category not found");
public class CategoryAlreadyExistsException(CategoryId categoryId)
    : CategoryException(categoryId, "Category already exists");
public class CategoryUnknownException(CategoryId categoryId, Exception innerException) 
    : CategoryException(categoryId, $"Unknown exception for category: {categoryId}"); 