namespace Hust_Medical.Repositories.Interfaces
{
    public interface IMedicineGroupRepo
    {
        Task<List<MedicineGroup>> GetMedicineGroups();
        Task<MedicineGroup> GetMedicineGroupById(string id);
        Task<List<MedicineGroup>> GetMedicineGroupByName(string name);
        Task<MedicineGroup> CreateMedicineGroup(MedicineGroup medicineGroup);
        Task ModifyMedicineGroupById(MedicineGroup medicineGroup);
    }
}