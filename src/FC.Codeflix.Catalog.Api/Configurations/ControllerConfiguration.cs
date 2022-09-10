namespace FC.Codeflix.Catalog.Api.Configurations;

public  static class ControllerConfiguration
{
    public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
    {
        
        services.AddSwaggerGen();
        services.AddDocumentation();
        
        return services;
    }
    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    public static WebApplication UseDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        return app;
    }
}
