using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;


namespace HemoTrack.Models
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get; set;}

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BlogName {get; set;} = null!;

        public string Title { get; set; }
        public string Content { get; set; }

        //Time based properties
        public DateTime CreateTime { get; set; }
    }
 
      // The class to manage the data-sources
    // public class PostManager
    // {
    //     // Define the members
    //     private static string PostsFile = HttpContext.Current.Server.MapPath(".\\Posts\\Posts.json");
    //     private static List<Blog> posts = new List<Blog>();

    //     // The CRUD functions
    //     public static void Create(string postJson)
    //     {
    //         var obj = JsonConvert.DeserializeObject<Blog>(postJson);

    //         if(posts.Count > 0)
    //         {
    //             posts = (from post in posts
    //                     orderby post.CreateTime
    //                     select post).ToList();
    //             obj.ID = posts.Last().ID + 1;
    //         } else
    //         {
    //             obj.ID = 1;
    //         }
    //     }

    //     posts.Add(obj);
    //     save();
    // }

    // [HttpGet]
    // public static List<Blog> Read()
    // {
    //     // Check if the file exists.
    //     if(!File.Exists(PostsFile))
    //     {
    //         File.Create(PostsFile).Close();
    //         File.WriteAllText(PostsFile, "[]"); // Create the file if it doesn't exist.
    //     }
    //     posts = JsonConvert.DeserializeObject<List<Blog>>(File.ReadAllText(PostsFile));
    //     return posts;
    // }

    // public static void Update(int id, string postJson)
    // {
    //     Delete(id);
    //     Create(postJson);
    //     save();
    // }

    // public static void Delete(int id)
    // {
    //     posts.Remove(posts.Find(x => x.ID == id));
    //     save();
    // }

    // // Output function
    // private static void save()
    // {
    //     File.WriteAllText(PostsFile, JsonConvert.SerializeObject(posts));
    // }
}