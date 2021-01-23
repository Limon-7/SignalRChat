using Chat.Data.Context;
using Chat.Data.Data_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data.Data_Repositories
{
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{
		private readonly DbSet<TEntity> _entity;

        public BaseRepository(ChatContext context)
        {
            _entity = context.Set<TEntity>();
        }
		public async Task<TEntity> GetById(int id)
		{
			return await _entity.FindAsync(id);
		}

		public async Task<IEnumerable<TEntity>> GetAll()
		{

			return await _entity.ToListAsync().ConfigureAwait(true);
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predict)
		{
			return _entity.Where(predict);
		}

		public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predict)
		{
			return _entity.FirstOrDefault(predict);
		}

		public void Add(TEntity entity)
		{
			_entity.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			_entity.AddRange(entities);
		}

		public void Remove(TEntity entity)
		{
			_entity.Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			_entity.RemoveRange(entities);
		}

		public void Modify(TEntity entity)
		{
			_entity.Update(entity);
		}

	}
}
