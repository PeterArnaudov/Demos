namespace SpecificationPattern.Interfaces
{
    public interface ISpecification<T>
        where T : BaseModel
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }
    }
}
