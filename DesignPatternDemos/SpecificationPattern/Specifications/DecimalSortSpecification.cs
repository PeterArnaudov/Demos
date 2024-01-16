using Microsoft.EntityFrameworkCore;
using SpecificationPattern.Extensions;

namespace SpecificationPattern.Specifications
{
    public class DecimalSortSpecification<T> : BaseSortSpecification<T>
        where T : BaseModel
    {
        public DecimalSortSpecification(
            Expression<Func<T, object?>> propertyExpression,
            bool isAscending = false)
        {
            var propertyName = propertyExpression.GetPropertyName();

            Criteria = x => EF.Property<decimal>(x, propertyName);
            IsAscending = isAscending;
        }

        public DecimalSortSpecification(
            string propertyName,
            bool isAscending = false)
        {
            Criteria = x => EF.Property<decimal>(x, propertyName);
            IsAscending = isAscending;
        }
    }
}
