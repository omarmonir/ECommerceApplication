namespace eCommerceApp.Domain.Interfaces
{
	public interface IGeneric<TEntity> where TEntity : class
	{
		Task<IEnumerable<TEntity>> GetAllAsync();

		Task<TEntity> GetByIdAsync(Guid id);
		Task<int> AddAsync(TEntity entity);
		
		Task<int> UpdateAsync(TEntity entity);

		Task<int> DeleteAsync(Guid id);
	}
}
