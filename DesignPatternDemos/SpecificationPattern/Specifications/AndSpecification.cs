namespace SpecificationPattern.Specifications
{
    public class AndSpecification<T> : BaseSpecification<T>
        where T : BaseModel
    {
        public AndSpecification(
            ISpecification<T> left,
            ISpecification<T> right)
        {
            var parameterExpression = Expression.Parameter(typeof(T));
            var expressionBody = Expression.AndAlso(left.Criteria.Body, right.Criteria.Body);
            expressionBody = (BinaryExpression)new ParameterReplacer(parameterExpression).Visit(expressionBody);

            var finalExpression = Expression.Lambda<Func<T, bool>>(expressionBody, parameterExpression);

            this.Criteria = finalExpression;
        }
    }
}
