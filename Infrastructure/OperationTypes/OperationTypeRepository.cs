using System.Linq.Expressions;
using System.Reflection;
using HealthCare.Domain.OperationTypes;
using HealthCare.Infrastructure;
using HealthCare.Infrastructure.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace HealthCare.Infraestructure.Staffs
{
    public class OperationTypeRepository : BaseRepository<OperationType, OperationTypeId>, IOperationTypeRepository
    {

        public OperationTypeRepository(HealthCareDbContext context) : base(context.OperationTypes)
        {
        }

        public async Task<OperationType> GetByIdEagerAsync(OperationTypeId id)
        {
            return await _objs
                .Include(ot => ot.Versions.Last()).FirstOrDefaultAsync(ot => ot.Id == id);
        }

        public async Task<List<OperationType>> ApplyQuery(IQueryCollection query)
        {
            IQueryable<OperationType> filteredQuery = _objs;

            foreach (var (key, value) in query)
            {
                if (string.IsNullOrEmpty(value)) continue;

                var parameter = Expression.Parameter(typeof(OperationType), "ot");
                var property = Expression.Property(parameter, key);

                if ( property.Type == typeof(string) )
                    filteredQuery = ApplyStringFilter(value, parameter, property, filteredQuery);
                if ( property.Type == typeof(bool) )
                    filteredQuery = ApplyBooleanFilter(value, parameter, property, filteredQuery);
            }

            return await filteredQuery.ToListAsync();
        }

        private IQueryable<OperationType> ApplyStringFilter(
            string value,
            ParameterExpression parameter,
            MemberExpression property,
            IQueryable<OperationType> query
        ) {
            var method = "Contains";
            if (value.StartsWith('*') && !value.EndsWith('*'))
            {
                method = "StartsWith";
                value = value.TrimStart('*');
            }
            else if (!value.StartsWith('*') && value.EndsWith('*'))
            {
                method = "EndsWith";
                value = value.TrimEnd('*');
            }
            else
            {
                value = value.Trim('*');
            }

            var expressionMethod = typeof(string).GetMethod(method, [typeof(string)]);
            var expression = Expression.Call(property, expressionMethod, Expression.Constant(value));

            var lambda = Expression.Lambda<Func<OperationType, bool>>(expression, parameter);

            return query.Where(lambda);
        }

        private IQueryable<OperationType> ApplyBooleanFilter(
            string value,
            ParameterExpression parameter,
            MemberExpression property,
            IQueryable<OperationType> query
        )
        {
            if (bool.TryParse(value, out var boolValue))
            {
                var equalsExpression = Expression.Equal(property, Expression.Constant(boolValue));
                var lambda = Expression.Lambda<Func<OperationType, bool>>(equalsExpression, parameter);

                return query.Where(lambda);
            }

            return query;
        }
    }
}