public record KeycloakSettings
{
    public const string ConfigSectionName = "Keycloak";

    public Uri Authority { get; init; } = null!;
    public bool RequireHttpsMetadata { get; init; } = false;

    public void Deconstruct(out string authority, out bool requireHttpsMetadata)
    {
        authority = Authority.ToString();
        requireHttpsMetadata = RequireHttpsMetadata;
    }
}