using System.ComponentModel.DataAnnotations;

namespace CachePractice.API.Contracts;

public record AddCustomerRequest(
    [Required]
    string Name,
    [Required]
    string Email);