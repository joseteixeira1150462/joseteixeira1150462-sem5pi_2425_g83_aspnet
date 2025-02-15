using System.Linq.Expressions;
using System.Reflection;
using HealthCare.Domain.OperationRequests;
using HealthCare.Infrastructure;
using HealthCare.Infrastructure.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace HealthCare.Infraestructure.OperationRequests
{
    public class OperationRequestRepository : BaseRepository<OperationRequest, OperationRequestId>, IOperationRequestRepository
    {

        public OperationRequestRepository(HealthCareDbContext context) : base(context.OperationRequest)
        {
        }
        public async Task<List<OperationRequest>> GetAllOperationRequest()
        {
            return await _objs
                .ToListAsync();
        }

        public async Task<OperationRequest> GetByIdEagerAsync(OperationRequestId id)
        {
            return await _objs
                .FirstOrDefaultAsync(ot => ot.Id == id);
        }

        public async Task<List<OperationRequest>> ApplyQuery(IQueryCollection query)
        {
            IQueryable<OperationRequest> filteredQuery = _objs;

            foreach (var (key, value) in query)
            {
                if (string.IsNullOrEmpty(value)) continue;

                var parameter = Expression.Parameter(typeof(OperationRequest), "ot");
                var property = Expression.Property(parameter, key);

                if ( property.Type == typeof(string) )
                    filteredQuery = ApplyStringFilter(value, parameter, property, filteredQuery);
                if ( property.Type == typeof(bool) )
                    filteredQuery = ApplyBooleanFilter(value, parameter, property, filteredQuery);
            }

            return await filteredQuery.ToListAsync();
        }

        private IQueryable<OperationRequest> ApplyStringFilter(
            string value,
            ParameterExpression parameter,
            MemberExpression property,
            IQueryable<OperationRequest> query
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

            var lambda = Expression.Lambda<Func<OperationRequest, bool>>(expression, parameter);

            return query.Where(lambda);
        }

        private IQueryable<OperationRequest> ApplyBooleanFilter(
            string value,
            ParameterExpression parameter,
            MemberExpression property,
            IQueryable<OperationRequest> query
        )
        {
            if (bool.TryParse(value, out var boolValue))
            {
                var equalsExpression = Expression.Equal(property, Expression.Constant(boolValue));
                var lambda = Expression.Lambda<Func<OperationRequest, bool>>(equalsExpression, parameter);

                return query.Where(lambda);
            }

            return query;
        }
    }
}