using System.ComponentModel.DataAnnotations;

namespace CachePractice.API.Contracts;

public record UpdateCustomerRequest(
    [Required]
    string Name,
    [Required]
    string Email);