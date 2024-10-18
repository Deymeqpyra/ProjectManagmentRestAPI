using Domain.TagsProjects;

namespace Domain.Tags;
public class Tag
{
    public TagId Id {get;}
    
    public string Name {get; private set; }

    public ICollection<TagsProject> TagsProjects { get; } = [];
}