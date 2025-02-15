using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;
using System;
using HealthCare.Domain.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HealthCare.Domain.Shared.Mailing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Newtonsoft.Json;
using HealthCare.Logs;






namespace HealthCare.Domain.Patients
{
    public class PatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        private readonly LogChangesService _logChangesService;
        //private readonly string _logFileName = "patient_update_logs.json";



        public PatientService(
        IUnitOfWork unitOfWork,
        IPatientRepository patientRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepository = patientRepository;
            _logChangesService = new LogChangesService();
        }

        //Retrieve a patient bi Id
        public async Task<PatientDto> GetPatientByIdAsync(PatientId id)
        {
            Patient patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                return null;
            }

            return new PatientDto
            {
                Id = patient.Id.AsGuid(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalRecordNumber = patient.MedicalRecordNumber.Number,
                Email = patient.Email.Address,
                Phone = patient.Phone.Value,
                HealthConditions = patient.HealthConditions,
                EmergencyPhone = patient.EmergencyPhone.Value,
                SequentialNumber = patient.SequentialNumber,
                UpdatesExecuted = patient.UpdatesExecuted
            };
        }

        //Add a new patient
        public async Task<PatientDto> AddPatientAsync(CreatingPatientDto dto)
        {
            var patient = await CreatePatientAsync(dto);
            if (patient.UpdatesExecuted < 0)
            {
                throw new ArgumentException("UpdatesExecuted cannot be negative.");
            }
            await _patientRepository.AddAsync(patient);
            await _unitOfWork.CommitAsync();

            return new PatientDto
            {
                Id = patient.Id.AsGuid(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalRecordNumber = patient.MedicalRecordNumber.Number,
                Email = patient.Email.Address,
                Phone = patient.Phone.Value,
                HealthConditions = patient.HealthConditions,
                EmergencyPhone = patient.EmergencyPhone.Value,
                SequentialNumber = patient.SequentialNumber,
                UpdatesExecuted = patient.UpdatesExecuted
            };
        }

        public async Task<PatientDto> UpdatePatientAsync(EditingPatientDto dto)
        {
            var patient = await _patientRepository.GetByEmailAsync(dto.Email);
            if (patient == null)
            {
                throw new InvalidOperationException("Patient not found, operation is invalid");
            }

            PatientDto oldPatientDataDto = MapPatientToDto(patient);

            UpdatePatientProperties(patient, dto);

            // Verificar se houve alterações reais
            if (!IsPatientUpdated(patient, oldPatientDataDto))
            {
                throw new InvalidOperationException("No changes were made, update request cannot be processed.");
            }
            //Update patient's properties
            await _patientRepository.UpdateAsync(patient);
            await _unitOfWork.CommitAsync();

            var newPatientDataDto = new PatientDto
            {
                Id = patient.Id.AsGuid(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalRecordNumber = patient.MedicalRecordNumber.Number,
                Email = patient.Email.Address,
                Phone = patient.Phone.Value,
                HealthConditions = patient.HealthConditions,
                EmergencyPhone = patient.EmergencyPhone.Value,
                SequentialNumber = patient.SequentialNumber,
                UpdatesExecuted = patient.UpdatesExecuted
            };
            
            // Log the changes made to the patient
            await _logChangesService.AppendPatientLogAsync(oldPatientDataDto,newPatientDataDto ,patient.Id);

            return newPatientDataDto;
        }

        public async Task<List<ListPatientDto>> GetAllPatientsAsync(){
            var list = await _patientRepository.GetAllPatients();

            return list.Select(patient => new ListPatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalRecordNumber = patient.MedicalRecordNumber,
                Email = patient.Email,
                Phone = patient.Phone,
                EmergencyPhone = patient.EmergencyPhone,
                SequentialNumber = patient.SequentialNumber,
                UpdatesExecuted = patient.UpdatesExecuted
            }).ToList();
        }

        public async Task<Guid> GetPatientIdByUserId(UserId userId){
            var patientId = await _patientRepository.GetPatientIdByUserIdAsync(userId);
            return patientId;
        }

        // Helper method to create a patient
        private async Task<Patient> CreatePatientAsync(CreatingPatientDto dto)
        {
            var patientPhone = new PatientPhone(dto.Phone);
            var emergencyPhone = new PatientPhone(dto.EmergencyPhone);

            // Get the last sequential number and increment it
            int sequentialNumber = await _patientRepository.getLastSequencialNumber() + 1;

            // Create a medical record number from the current month and the sequential number
            var medicalRecordNumber = PatientMedicalRecordNumber.CreateFromPlainText(dto.BirthDate.Month, sequentialNumber);

            var patientEmail = new PatientEmail(dto.Email);

            int patientUpdatesExecuted = 0;

            // Create the patient entity
            var patient = new Patient(
                dto.FirstName,
                dto.LastName,
                dto.BirthDate,
                dto.Gender,
                medicalRecordNumber,
                patientEmail,
                patientPhone,
                emergencyPhone,
                sequentialNumber,
                patientUpdatesExecuted
            );

            return patient;
        }

        private void UpdatePatientProperties(Patient patient, EditingPatientDto dto)
        {
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            bool hasChanges = false;

            // Verificar e atualizar as propriedades do paciente somente se houver alteração
            if (!string.IsNullOrWhiteSpace(dto.FirstName) && dto.FirstName != patient.FirstName)
            {
                patient.FirstName = dto.FirstName;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.LastName) && dto.LastName != patient.LastName)
            {
                patient.LastName = dto.LastName;
                hasChanges = true;
            }

            if (dto.BirthDate != default && dto.BirthDate != patient.BirthDate)
            {
                patient.BirthDate = dto.BirthDate;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Gender) && dto.Gender != patient.Gender)
            {
                patient.Gender = dto.Gender;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Phone) && dto.Phone != patient.Phone.Value)
            {
                patient.Phone = new PatientPhone(dto.Phone);
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.EmergencyPhone) && dto.EmergencyPhone != patient.EmergencyPhone.Value)
            {
                patient.EmergencyPhone = new PatientPhone(dto.EmergencyPhone);
                hasChanges = true;
            }

            //Fullfill FulName property based on First and Last Names.
            if (hasChanges)
            {
                patient.FullName = $"{dto.FirstName} {dto.LastName}";
                patient.IncrementUpdateAttempts();
            }
        }

        //--------------------------Helper Methods Section-----------------------------
        private bool IsPatientUpdated(Patient patient, PatientDto oldPatientDataDto)
        {

            return patient.FirstName != oldPatientDataDto.FirstName ||
                   patient.LastName != oldPatientDataDto.LastName ||
                   patient.BirthDate != oldPatientDataDto.BirthDate ||
                   patient.Gender != oldPatientDataDto.Gender ||
                   patient.Email.Address != oldPatientDataDto.Email ||
                   patient.Phone.Value != oldPatientDataDto.Phone ||
                   patient.EmergencyPhone.Value != oldPatientDataDto.EmergencyPhone;
        }

        // Helper method to map a Patient to a PatientDto
        private PatientDto MapPatientToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id.AsGuid(),
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                FullName = patient.FullName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                MedicalRecordNumber = patient.MedicalRecordNumber.Number,
                Email = patient.Email.Address,
                Phone = patient.Phone.Value,
                HealthConditions = patient.HealthConditions,
                EmergencyPhone = patient.EmergencyPhone.Value,
                SequentialNumber = patient.SequentialNumber,
                UpdatesExecuted = patient.UpdatesExecuted
            };
        }
    }
}