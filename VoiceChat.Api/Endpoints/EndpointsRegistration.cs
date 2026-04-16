using VoiceChat.Data.Repositories;

namespace VoiceChat.Api.Endpoints
{

    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute { }



    public static class EndpointsRegistration
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            app.AddServerEndpoints();
        }
    }
}
