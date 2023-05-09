﻿namespace Patient_Health_Management_System.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IMongoCollection<User> _user;

        public UserRepo(MongoDbSetup mongoDbSetup)
        {
            _user = mongoDbSetup.GetDatabase().GetCollection<User>("user_extra_info");
        }

        public async Task<User> GetUserByUserId(string userId)
        {
            return await _user.Find(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUser(User User)
        {
            await _user.InsertOneAsync(User);
            return User;
        }

        public async Task ModifyUserByUserId(User User)
        {
            await _user.ReplaceOneAsync(u => u.Id == User.Id, User);
        }
    }
}