using System.ComponentModel.DataAnnotations;

namespace HealthCare.Domain.Users
{
    public class UpdateUserEmailDto {
    public UserEmail CurrEmail {get;set;}
    public UserEmail NewEmail {get;set;}
    }
}