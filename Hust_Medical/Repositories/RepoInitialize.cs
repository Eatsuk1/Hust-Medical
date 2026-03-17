namespace Hust_Medical.Repositories
{
    public class RepoInitialize
    {
        private readonly IMongoClient _client;
        private readonly IConfiguration _configuration;

        public RepoInitialize(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = _configuration["MongoDb:ConnectionString"];
            _client = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase("patient_health_db");
        }
    }
}
