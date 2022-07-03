using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Data.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _collection;

        public TodoRepository(IOptions<TodoDatabaseSettings> todoDatabaseSettings)
        {
            var mongoClient = new MongoClient(todoDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(todoDatabaseSettings.Value.DatabaseName);

            if (!mongoDatabase.ListCollectionNames().ToList().Contains(todoDatabaseSettings.Value.TodoCollectionName))
                mongoDatabase.CreateCollection(todoDatabaseSettings.Value.TodoCollectionName);

            _collection = mongoDatabase.GetCollection<Todo>(todoDatabaseSettings.Value.TodoCollectionName);
        }

        public async Task<List<Todo>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Todo> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Todo newTodo) =>
            await _collection.InsertOneAsync(newTodo);

        public async Task UpdateAsync(string id, Todo updateTodo) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updateTodo);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}