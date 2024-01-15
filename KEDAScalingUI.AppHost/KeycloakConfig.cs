namespace KEDAScalingUI.AppHost;

public class KeycloakConfig
{
    public string DB_VENDOR { get; set; } = null!;
    public string DB_HOST { get; set; } = null!;
    public string DB_PORT { get; set; } = null!;
    public string DB_DATABASE { get; set; } = null!;
    public string DB_USER { get; set; } = null!;
    public string DB_PASSWORD { get; set; } = null!;
    public string KEYCLOAK_USER { get; set; } = null!;
    public string KEYCLOAK_PASSWORD { get; set; } = null!;
    public string KC_HOSTNAME_PORT { get; set; } = null!;
}
