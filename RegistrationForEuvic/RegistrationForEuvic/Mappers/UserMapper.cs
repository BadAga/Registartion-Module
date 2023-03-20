using RegistrationForEuvic.Models.DTOs;

namespace RegistrationForEuvic.Models.Mappers
{
    public class UserMapper
    {
        public static User RegisterDtoToUser(ref RegisterDto dto,ref int id,String role="User")
        {
            if(dto == null)
            {
                return null;
            }

            return new User()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Pesel = dto.Pesel,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                Age = dto.Age,
                UserId = id,
                Role = role,
                PowerUsageAvg=dto.PowerUsageAvg,
            };
        }
    }
}
