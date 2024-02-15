using HemoTrack.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HemoTrack.Services;

public class BlogsService
{
    private readonly IMongoCollection<Blog> _blogsCollection;

    public BlogsService(
        IOptions<BlogStoreDatabaseSettings> blogStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            blogStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            blogStoreDatabaseSettings.Value.DatabaseName);

        _blogsCollection = mongoDatabase.GetCollection<Blog>(
            blogStoreDatabaseSettings.Value.BlogsCollectionName);
    }

    public async Task<List<Blog>> GetAsync() =>
        await _blogsCollection.Find(_ => true).ToListAsync();

    public async Task<Blog?> GetAsync(string id) =>
        await _blogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Blog newBlog) =>
        await _blogsCollection.InsertOneAsync(newBlog);

    public async Task UpdateAsync(string id, Blog updatedBlog) =>
        await _blogsCollection.ReplaceOneAsync(x => x.Id == id, updatedBlog);

    public async Task RemoveAsync(string id) =>
        await _blogsCollection.DeleteOneAsync(x => x.Id == id);
}