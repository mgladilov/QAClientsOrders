using QAClientsOrders.Data.Models;

namespace QAClientsOrders.Data.Repositories.Interfaces;

public interface IRepository<T> where T: BaseEntity
{
    public T Add(T item);
    public List<T> GetAll();
    public T GetById(int id);
    public void Update(T item);
    public void Delete(T item);
}