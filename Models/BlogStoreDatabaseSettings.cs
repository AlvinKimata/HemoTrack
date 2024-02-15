namespace HemoTrack.Models;

public class BlogStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName {get; set;} = null!;
    public string BlogsCollectionName {get; set;} = null!;
}