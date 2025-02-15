using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Shared.TimeSlot;
using HealthCare.Domain.Users;

namespace HealthCare.Domain.Staffs
{
    public class Staff : Entity<StaffId>, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public StaffLicenseNumber LicenseNumber { get; private set; }
        public StaffSpecialization Specialization { get; private set; }
        public StaffPhone Phone { get; private set; }
        public StaffEmail Email { get; private set; }
        public int SequencialNumber { get; private set; }
        public List<TimeSlot> AvailabilitySlots { get; private set; } = new List<TimeSlot>();
        public UserId UserId { get; private set; }
        public virtual User User { get; private set; }
        public bool IsActive { get; private set; }
        public Staff()
        {

            IsActive = true;
        }

        public Staff(
            string firstName,
            string lastName,
            StaffSpecialization specialization,
            StaffPhone phone,
            StaffEmail email,
            StaffLicenseNumber licenseNumber,
            int sequencialNumber
        )
        {
            Id = new StaffId(Guid.NewGuid());

            FirstName = firstName;
            LastName = lastName;
            FullName = $"{FirstName} {LastName}";
            Specialization = specialization;
            Phone = phone;
            Email = email;
            LicenseNumber = licenseNumber;
            SequencialNumber = sequencialNumber;
            IsActive = true;
        }

        public void AddAvailabilitySlot(TimeSlot newSlot)
        {
            // Verifica se o novo slot entra em conflito com algum slot existente
            if (AvailabilitySlots.Any())
            {
                if (AvailabilitySlots.Any(slot => slot.ConflictsWith(newSlot)))
                {
                    throw new BusinessRuleValidationException("This time slot conflicts with an existing availability slot.");
                }
            }

            AvailabilitySlots.Add(newSlot);
        }

        public void AssociateUser(User user)
        {
            if (User != null)
            {
                throw new BusinessRuleValidationException("The Staff profile with e-mail: " + user.Email.Address + " already has a User");
            }

            if (user.Email.Address != Email.Value)
            {
                throw new BusinessRuleValidationException("There isn\'t a Staff profile with e-mail address: " + user.Email.Address);
            }

            UserId = user.Id;
            User = user;
        }

        public void ChangePhone(StaffPhone newPhone)
        {
            if (newPhone == null)
            {
                throw new ArgumentNullException(nameof(newPhone), "Phone cannot be null");
            }

            Phone = newPhone;
        }

        public void ClearAvailabilitySlots()
        {
            AvailabilitySlots.Clear();
        }

        public void MarkAsInactive()
        {
            if (!IsActive)
            {
                throw new BusinessRuleValidationException("The staff profile is already inactive.");
            }
            IsActive = false;
        }
    }
}