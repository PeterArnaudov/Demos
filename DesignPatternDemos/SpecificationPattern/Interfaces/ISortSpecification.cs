namespace SpecificationPattern.Interfaces
{
    public interface ISortSpecification<T>
        where T : BaseModel
    {
        Expression<Func<T, object>> Criteria { get; }

        public bool? IsAscending { get; set; }

        List<Expression<Func<T, object>>> Includes { get; }
    }
}
