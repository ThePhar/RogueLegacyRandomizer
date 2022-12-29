namespace Randomizer;

public record ConnectionInfo
{
    public string Hostname { get; init; } = "archipelago.gg";
    public ushort Port     { get; init; } = 38281;
    public string Name     { get; init; } = "Sir Lee";
    public string Password { get; init; } = "";
}