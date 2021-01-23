using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data.Data_Interfaces
{
	public interface IBaseRepository<TEntity> where TEntity : class
	{
		
		Task<TEntity> GetById(int id);
		Task<IEnumerable<TEntity>> GetAll();

		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predict);
		TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predict);
		

		void Modify(TEntity entity);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);


	}
}
