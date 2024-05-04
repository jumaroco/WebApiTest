using Domain;

namespace Web.Extensions;

public static class PoliciesConfiguration
{
    public static IServiceCollection AddPoliciesServices
    (
        this IServiceCollection services
    )
    {
        services.AddAuthorization( opt =>
        {
            opt.AddPolicy(
                   PolicyMaster.TAREA_READ, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TAREA_READ
                            )
                        )
            );

            opt.AddPolicy(
                   PolicyMaster.TAREA_WRITE, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TAREA_WRITE
                            )
                        )
            );

            opt.AddPolicy(
                   PolicyMaster.TAREA_DELETE, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TAREA_DELETE
                            )
                        )
            );

            opt.AddPolicy(
                   PolicyMaster.TAREA_UPDATE, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == CustomClaims.POLICIES && c.Value == PolicyMaster.TAREA_UPDATE
                            )
                        )
            );

        }
        );

        return services;
    }
}
