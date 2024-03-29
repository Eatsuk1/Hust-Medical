﻿namespace Hust_Medical.Repositories
{
    public class MedicineRepo : IMedicineRepo, IMedicineGroupRepo
    {
        private readonly IMongoCollection<Medicine> _medicines;
        private readonly IMongoCollection<MedicineGroup> _medicineGroups;

        public MedicineRepo(RepoInitialize mongoDbSetup)
        {
            _medicines = mongoDbSetup.GetDatabase().GetCollection<Medicine>("medicine");
            _medicineGroups = mongoDbSetup.GetDatabase().GetCollection<MedicineGroup>("medicine_group");
        }

        #region medicine
        public async Task<List<Medicine>> GetMedicines()
        {
            return await _medicines.Find(medicine => !medicine.IsDeleted).SortByDescending(medicine => medicine.Id).ToListAsync();
        }

        public async Task<List<Medicine>> GetMedicinesByPage(int page, int pageSize)
        {
            return await _medicines.Find(medicine => !medicine.IsDeleted).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<Medicine> GetMedicineById(string id)
        {
            return await _medicines.Find(medicine => medicine.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Medicine> GetMedicineByMedicineId(string medicineId)
        {
            return await _medicines.Find(medicine => medicine.MedicineId == medicineId).FirstOrDefaultAsync();
        }

        public async Task<List<Medicine>> GetMedicineByKeyword(string keyword)
        {
            var filter = Builders<Medicine>.Filter.And(Builders<Medicine>.Filter.Text(keyword, new TextSearchOptions { CaseSensitive = false }), Builders<Medicine>.Filter.Eq(medicine => medicine.IsDeleted, false));
            return await _medicines.Find(filter).ToListAsync();
        }

        public async Task<List<Medicine>> GetMedicinesByName(string name)
        {
            return await _medicines.Find(medicine => medicine.Name == name && !medicine.IsDeleted).ToListAsync();
        }

        public async Task<string> GetLastMedicineId()
        {
            var projection = Builders<Medicine>.Projection.Include(medicine => medicine.MedicineId);
            var lastMedicine = await _medicines.Find(_ => true).Project(projection).SortByDescending(medicine => medicine.MedicineId).Limit(1).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<Medicine>(lastMedicine).MedicineId;
        }

        public async Task<Medicine> CreateMedicine(Medicine medicine)
        {
            await _medicines.InsertOneAsync(medicine);
            return medicine;
        }

        public async Task ModifyMedicineById(Medicine medicine)
        {
            await _medicines.ReplaceOneAsync(m => m.Id == medicine.Id, medicine);
        }

        public async Task<List<Medicine>> GetMedicineByGroupName(string groupName)
        {
            return await _medicines.Find(medicine => medicine.GroupName == groupName).ToListAsync();
        }

        public async Task<List<Medicine>> ImportMedicineExcel(List<Medicine> medicines)
        {
            await _medicines.InsertManyAsync(medicines);
            return medicines;
        }

        public async Task DeleteMultiMedicinesById(List<Medicine> medicines)
        {
            var deletes = new List<WriteModel<Medicine>>();
            foreach (var medicine in medicines)
            {
                deletes.Add(new ReplaceOneModel<Medicine>(Builders<Medicine>.Filter.Eq(m => m.Id, medicine.Id), medicine));
            }
            await _medicines.BulkWriteAsync(deletes, new BulkWriteOptions() { IsOrdered = false });
        }
        #endregion

        #region medicine_group
        public async Task<List<MedicineGroup>> GetMedicineGroups()
        {
            return await _medicineGroups.Find(medicineGroup => !medicineGroup.IsDeleted).SortByDescending(medicineGroup => medicineGroup.Id).ToListAsync();
        }

        public async Task<MedicineGroup> GetMedicineGroupById(string id)
        {
            return await _medicineGroups.Find(medicineGroup => medicineGroup.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<MedicineGroup>> GetMedicineGroupByName(string name)
        {
            var filter = Builders<MedicineGroup>.Filter.And(Builders<MedicineGroup>.Filter.Text(name, new TextSearchOptions { CaseSensitive = false }), Builders<MedicineGroup>.Filter.Eq(medicineGroup => medicineGroup.IsDeleted, false));
            return await _medicineGroups.Find(filter).ToListAsync();
        }

        public async Task<MedicineGroup> CreateMedicineGroup(MedicineGroup medicineGroup)
        {
            await _medicineGroups.InsertOneAsync(medicineGroup);
            return medicineGroup;
        }

        public async Task ModifyMedicineGroupById(MedicineGroup medicineGroup)
        {
            await _medicineGroups.ReplaceOneAsync(m => m.Id == medicineGroup.Id, medicineGroup);
        }
        #endregion
    }
}
