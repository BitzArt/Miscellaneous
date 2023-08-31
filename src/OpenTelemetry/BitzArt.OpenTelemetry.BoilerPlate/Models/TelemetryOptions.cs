namespace BitzArt.OpenTelemetry.BoilerPlate;

public class TelemetryOptions
{
    public bool EnrichOutboundHttpRequests;

	public TelemetryOptions()
	{
		EnrichOutboundHttpRequests = false;
	}
}
