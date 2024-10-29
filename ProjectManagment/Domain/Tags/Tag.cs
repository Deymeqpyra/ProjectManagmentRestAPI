using Domain.TagsProjects;

namespace Domain.Tags;
public class Tag
{
    public TagId Id {get;}
    
    public string Name {get; private set; }

    public ICollection<TagsProject> TagsProjects { get; } = [];

    private Tag(TagId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Tag New(TagId id, string name)
        => new(id, name);

    public void UpdateDetails(string tagName)
    {
        Name = tagName;
    }
}