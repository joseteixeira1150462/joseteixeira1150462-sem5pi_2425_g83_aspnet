using System;
using System.Collections.Generic;
using System.Linq;
using HealthCare.Domain.Staffs;

namespace HealthCare.Domain.OperationTypes
{
    public static class OperationTypeMapper
    {
        public static OperationTypeDto DomainToDto(OperationType operationType)
        {
            var currentVersion = operationType.Versions.Last();
            var specializationDtoList = new List<OperationTypeSpecializationDto>();

            foreach (var specialization in currentVersion.Specializations)
            {
                var specializationDto = new OperationTypeSpecializationDto(
                    specialization.Id.AsGuid(),
                    specialization.Quantity,
                    specialization.Role.ToString(),
                    specialization.Specialization.ToString()
                );

                specializationDtoList.Add(specializationDto);
            }

            return new OperationTypeDto(
                operationType.Id.AsGuid(),
                operationType.Name,
                operationType.Active,
                currentVersion.Duration.Preparation,
                currentVersion.Duration.Operation,
                currentVersion.Duration.Cleaning,
                specializationDtoList
            );
        }

        public static OperationType CreatingDtoToDomain(OperationTypeId id, CreatingOperationTypeDto dto)
        {
            var operationType = new OperationType(dto.Name);

            var newVersion = new OperationTypeVersion(
                dto.Preparation,
                dto.Operation,
                dto.Cleaning
            );

            foreach (var specializationDto in dto.Specializations)
            {
                var specialization = new OperationTypeSpecialization(
                    specializationDto.Quantity,
                    specializationDto.Role,
                    specializationDto.Specialization
                );

                newVersion.AddSpecialization(specialization);
            }

            operationType.AddVersion(newVersion);

            return operationType;
        }
    }
}