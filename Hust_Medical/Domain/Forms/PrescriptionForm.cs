﻿namespace Hust_Medical.Domain.Forms
{
    public class PrescriptionForm
    {
        [Required]
        public string PatientId { get; set; }
        public string MedicalExaminationId { get; set; }
        public string? Note { get; set; }

        [Required]
        public List<MedicinePrescribed> MedicinePrescribed { get; set; }
    }
}
