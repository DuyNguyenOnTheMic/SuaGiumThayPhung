using CAPTimeTableIT.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTimeTableIT.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext context;
        private DbSet<T> entities;
        public Repository(DbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
        public T Get(int id)
        {
            return entities.FirstOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var entity = entities.Find(id);
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            //context.SaveChanges(): tra ve so luong record duoc thuc hien thanh cong duoi database
            //VD: 0: khong co record nao duoc cap nhat; 1: 1 record duoc cap nhat
            //Trong truong hop nay: khi xoa 1 phan tu, C# se goi cau lenh xoa 1 record xuong database, neu xoa thanh cong, sql server se tra ve 1.
            var result =  context.SaveChanges();
            return result > 0;
        }
    }
}
