namespace CareLink.Application.Notifications
{
    
    public static class TaskExtensions
    {
        public static async Task<T> CastTask<T>(Task<object> task)
        {
            var result = await task;
            return (T)result!;
        }
    }
}