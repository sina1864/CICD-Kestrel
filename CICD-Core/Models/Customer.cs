using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CICD_Core.Models;

public class Customer
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement]
    [Required(ErrorMessage = "CustomerId can't be null or empty")]
    public int CustomerId { get; set; }

    [BsonElement]
    [Required(ErrorMessage = "Name can't be null or empty")]
    public string Name { get; set; }

    [BsonElement]
    [Required(ErrorMessage = "Name can't be null or empty")]
    [Range(18, 99, ErrorMessage = "Age must be between 18 and 99")]
    public int Age { get; set; }

    [BsonElement]
    [Required(ErrorMessage = "Salary can't be null or empty")]
    [Range(1000, 10000, ErrorMessage = "Salary must be between 1000 and 10000")]
    public int Salary { get; set; }
}
