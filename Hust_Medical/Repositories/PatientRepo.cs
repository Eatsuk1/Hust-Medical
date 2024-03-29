﻿
namespace Hust_Medical.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        private readonly IMongoCollection<Patient> _patient;

        public PatientRepo(RepoInitialize mongoDbSetup)
        {
            _patient = mongoDbSetup.GetDatabase().GetCollection<Patient>("patient");
        }

        public async Task CreatePatient(Patient patient)
        {
            try
            {
                await _patient.InsertOneAsync(patient);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Patient> GetPatientById(string id)
        {
            try
            {
                return await _patient.Find(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Patient>> GetPatients()
        {
            try
            {
                return await _patient.Find(p => !p.IsDeleted).SortByDescending(p => p.Id).ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> GetLastPatientId()
        {
            try
            {
                var projection = Builders<Patient>.Projection.Include(patient => patient.PatientId);
                var lastPatient = await _patient.Find(_ => true).Project(projection).SortByDescending(patient => patient.PatientId).Limit(1).FirstOrDefaultAsync();
                return BsonSerializer.Deserialize<Patient>(lastPatient).PatientId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task ModifyPatientById(Patient patient)
        {
            try
            {
                await _patient.ReplaceOneAsync(p => p.Id == patient.Id, patient);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<long> GetNumberPatientsByCreatedDay(DateTime date)
        {
            try
            {
                var filter = Builders<Patient>.Filter;
                var filterDate = filter.Gte(p => p.CreatedAt, date.Date) & filter.Lt(p => p.CreatedAt, date.Date.AddDays(1));
                return await _patient.Find(filterDate).CountDocumentsAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Patient>> GetPatientsByDoctorId(string doctorId)
        {
            try
            {
                var filter = Builders<Patient>.Filter;
                var filterDoctorId = filter.Eq(p => p.CreatedBy, doctorId) & filter.Eq(p => p.IsDeleted, false);
                return await _patient.Find(filterDoctorId).SortByDescending(p => p.Id).ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<long> GetNumberPatients()
        {
            try
            {
                return await _patient.Find(p => !p.IsDeleted).CountDocumentsAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Patient> GetPatientByIDNumber(string iDNumber)
        {
            try
            {
                return await _patient.Find(p => p.IDNumber.Equals(iDNumber)).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
