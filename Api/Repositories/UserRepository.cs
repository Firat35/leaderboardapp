using Api.Models;

using MongoDB.Bson;
using MongoDB.Driver;

using System.Numerics;

namespace Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserRepository(IMongoDatabase mongoDatabase)
        {
            _userCollection = mongoDatabase.GetCollection<User>("user");
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync(); ;
        }
        public async Task<bool> UpdateManyAsync(List<User> userList)
        { 
            var listWrites = new List<WriteModel<User>>();
            foreach (var user in userList)
            {
                var filterDefinition = Builders<User>
                    .Filter
                    .Eq(u => u.Id, user.Id);
                var updateDefinition = Builders<User>
                    .Update
                    .Set(u => u.Prizes, user.Prizes)
                    .Set(u => u.TotalPoint, user.TotalPoint);
                listWrites.Add(new UpdateManyModel<User>(filterDefinition, updateDefinition));
            }
            
            var result = await _userCollection.BulkWriteAsync(listWrites);
            return result.IsAcknowledged;
            //return await _userCollection.FindOneAndReplaceAsync(x => x.Id == user.Id, user) != null;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);

            return user;
        }

        public async Task AddRangeAsync(List<User> userList)
        {
            await _userCollection.InsertManyAsync(userList);
        }
    }
}
