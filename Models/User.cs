using Enum;
namespace CoreProject.Models;

public class User : GenericId
{
        public string Password { get; set; }
        public UserType userType{get;set;}
}