using CachePractice.Domain.Models;
using CachePractice.Persistence;
using Microsoft.Extensions.Caching.Memory;

namespace CachePractice.Logic.Services;

public class CustomerService
{
    const string AllCustomersCacheKey = "all_customers";
    const string CustomerCacheKeyPrefix = "customer_";
    
    private readonly CustomerRepository _repository;
    private readonly IMemoryCache _cache;

    public CustomerService(CustomerRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Customer>?> GetAllCustomersAsync()
    {
        var isCached = _cache.TryGetValue(AllCustomersCacheKey, out IReadOnlyList<Customer>? customers);
        if (!isCached)
        {
            customers = await _repository.GetAllCustomersAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(AllCustomersCacheKey, customers, cacheEntryOptions);
        }

        return customers;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        string cacheKey = CustomerCacheKeyPrefix + id.ToString();
        bool isCached = _cache.TryGetValue(cacheKey, out Customer? customer);
        if (!isCached)
        {
            customer = await _repository.GetCustomerByIdAsync(id);

            if (customer != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));


                _cache.Set(cacheKey, customer, cacheEntryOptions);
            }
        }

        return customer;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _repository.AddCustomerAsync(customer);
        _cache.Remove(AllCustomersCacheKey);
    }

    public async Task<bool> UpdateCustomerAsync(int id, Customer customer)
    {
        var customerToUpdate = await _repository.GetCustomerByIdAsync(id);
        if (customerToUpdate == null)
        {
            return false;
        }
        
        customerToUpdate.Email = customer.Email;
        customerToUpdate.Name = customer.Name;
        
        await _repository.UpdateCustomerAsync(customerToUpdate);
        
        _cache.Remove(CustomerCacheKeyPrefix + customerToUpdate.Id);
        _cache.Remove(AllCustomersCacheKey);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customerToDelete = await _repository.GetCustomerByIdAsync(id);
        if (customerToDelete == null)
        {
            return false;
        }
        
        await _repository.DeleteCustomerAsync(customerToDelete);
        _cache.Remove(CustomerCacheKeyPrefix + customerToDelete.Id);
        _cache.Remove(AllCustomersCacheKey);
        return true;
    }
}