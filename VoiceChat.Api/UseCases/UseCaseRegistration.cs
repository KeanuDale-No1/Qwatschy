namespace VoiceChat.Api.UseCases
{
    public static class UseCaseRegistration
    {
        public static void AddUsecases(this IServiceCollection services)
        {
            var assembly = typeof(IUseCase<,>).Assembly;

            var useCaseTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Implementation = t,
                    Interfaces = t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUseCase<,>))
                })
                .Where(x => x.Interfaces.Any());

            foreach (var type in useCaseTypes)
            {
                foreach (var iface in type.Interfaces)
                {
                    services.AddScoped(iface, type.Implementation);
                }
            }
        }
    }
}
