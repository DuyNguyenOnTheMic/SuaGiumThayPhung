using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Only use a small data size
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool Delete(int id);
    }
}
