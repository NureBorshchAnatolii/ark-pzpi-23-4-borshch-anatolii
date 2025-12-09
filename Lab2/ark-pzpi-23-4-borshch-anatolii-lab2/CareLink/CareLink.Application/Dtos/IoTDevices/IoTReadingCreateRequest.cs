namespace CareLink.Application.Dtos.IoTDevices
{
    public record IoTReadingCreateRequest(
        DateTime ReadDateTime,
        long Pulse,
        long ActivityLevel,
        int Temperature,
        long DeviceId,
        long UserId
        );
}