namespace Hust_Medical.Repositories
{
    public class RepoInitialize
    {
        //private readonly IKeyVaultService _keyVaultService;
        private readonly IMongoClient _client;

        public RepoInitialize(IConfiguration configuration)
        {
            //_keyVaultService = keyVaultService;
            var connectionString = configuration["MongoDbConnectionString"];
            _client = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase("patient_health_db");
        }
    }
}
