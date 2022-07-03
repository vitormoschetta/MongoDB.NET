using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models;
public class Todo
{
    public Todo()
    {
        Id = Guid.NewGuid().ToString();
    }

    public Todo(string title)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
    }

    [BsonId]    
    public string Id { get; private set; }
    public string Title { get; private set; }
    public bool Done { get; private set; }

    public void Update(string title, bool done = false)
    {
        Title = title;
        Done = done;
    }
}