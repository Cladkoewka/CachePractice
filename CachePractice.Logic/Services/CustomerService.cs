using System.Text.Json;
using CachePractice.Domain.Models;
using CachePractice.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace CachePractice.Logic.Services;

public class CustomerService
{
    const string AllCustomersCacheKey = "all_customers";
    const string CustomerCacheKeyPrefix = "customer_";
    
    private readonly CustomerRepository _repository;
    private readonly IDistributedCache _cache;

    public CustomerService(CustomerRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Customer>?> GetAllCustomersAsync()
    {
        var cachedCustomers = await _cache.GetStringAsync(AllCustomersCacheKey);
        if (!string.IsNullOrEmpty(cachedCustomers))
        {
            return JsonSerializer.Deserialize<List<Customer>>(cachedCustomers);
        }

        var customers = await _repository.GetAllCustomersAsync();
        
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        await _cache.SetStringAsync(AllCustomersCacheKey, JsonSerializer.Serialize(customers), options);

        return customers;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        string cacheKey = CustomerCacheKeyPrefix + id.ToString();
        var cachedCustomer = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedCustomer))
        {
            return JsonSerializer.Deserialize<Customer>(cachedCustomer);
        }

        var customer = await _repository.GetCustomerByIdAsync(id);
        
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));
        
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(customer), options);

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