namespace CareLink.Application.Dtos
{
    public record UserRegisterDto(
        string Name,
        string Surname,
        string Role,
        string Email,
        string Password,
        DateTime BirthDate,
        string Address,
        string PhoneNumber
    );
}