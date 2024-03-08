namespace HemoTrack.Models;

public class BlogStoreDatabaseSettings
{
    public string ConnectionStrings { get; set; } = null!;
    public string DatabaseName {get; set;} = null!;
    public string BlogsCollectionName {get; set;} = null!;
}