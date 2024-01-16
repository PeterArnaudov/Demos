using SpecificationPattern.Enums;
using SpecificationPattern.Extensions;

namespace SpecificationPattern.Specifications
{
    public class DecimalSpecification<T> : BaseSpecification<T>
        where T : BaseModel
    {
        public DecimalSpecification(
            decimal? value,
            Expression<Func<T, object?>> propertyExpression,
            NumberFilterCondition condition = NumberFilterCondition.IsEqualTo)
        {
            var propertyName = propertyExpression.GetPropertyName();

            this.Criteria = GetCriteria(value, propertyName, condition);
        }

        public DecimalSpecification(
            IEnumerable<decimal?>? values,
            Expression<Func<T, object?>> propertyExpression,
            NumberFilterCondition condition = NumberFilterCondition.IsEqualTo,
            LogicalOperator logicalOperator = LogicalOperator.And)
        {
            var propertyName = propertyExpression.GetPropertyName();

            this.Criteria = GetCriteria(values, propertyName, condition, logicalOperator);
        }

        private Expression<Func<T, bool>> GetCriteria(
            decimal? value,
            string? propertyName,
            NumberFilterCondition condition)
        {
            var column = EFHelper.GetProperty<T, decimal>(propertyName);

            switch (condition)
            {
                case NumberFilterCondition.IsEqualTo:
                    return column.Compare(x => x == value);
                case NumberFilterCondition.IsHigherThan:
                    return column.Compare(x => x > value);
                case NumberFilterCondition.IsLessThan:
                    return column.Compare(x => x <= value);
                default:
                    return null;
            }
        }

        private Expression<Func<T, bool>> GetCriteria(
            IEnumerable<decimal?>? values,
            string? propertyName,
            NumberFilterCondition condition,
            LogicalOperator logicalOperator)
        {
            Expression<Func<T, bool>> complexQuery = p => logicalOperator == LogicalOperator.And;

            if (!values?.Any() ?? false)
            {
                return complexQuery;
            }

            foreach (var value in values)
            {
                var query = GetCriteria(value, propertyName, condition);

                complexQuery = GetComplexQuery(complexQuery, query, logicalOperator);
            }

            return complexQuery;
        }

        private Expression<Func<T, bool>> GetComplexQuery(
            Expression<Func<T, bool>> initialQuery,
            Expression<Func<T, bool>>? additionalQuery,
            LogicalOperator logicalOperator)
        {
            if (additionalQuery == null)
            {
                return initialQuery;
            }

            return logicalOperator switch
            {
                LogicalOperator.And
                    => initialQuery.And(additionalQuery),
                LogicalOperator.Or
                    => initialQuery.Or(additionalQuery),
                _ => initialQuery
            };
        }
    }
}
