using CICD_Core.Models;
using MongoDB.Driver;

namespace CICD_Core.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ICustomerContext _customerContext;
    public CustomerRepository(ICustomerContext customerContext)
    {
        _customerContext = customerContext;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        return await _customerContext.customersCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Customer> GetCustomerById(int id)
    {
        return await _customerContext.customersCollection.Find(x => x.CustomerId == id).FirstOrDefaultAsync();
    }

    public async Task CreateCustomer(Customer customer)
    {
        await _customerContext.customersCollection.InsertOneAsync(customer);
    }

    public async Task<bool> UpdateCustomer(Customer customer)
    {
        var updateResult = await _customerContext.customersCollection.ReplaceOneAsync(x => x.Id == customer.Id, customer);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCustomer(int id)
    {
        var deletedResult = await _customerContext.customersCollection.DeleteOneAsync(x => x.CustomerId == id);
        return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
    }
}

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer> GetCustomerById(int id);
    Task CreateCustomer(Customer customer);
    Task<bool> UpdateCustomer(Customer customer);
    Task<bool> DeleteCustomer(int id);
}
