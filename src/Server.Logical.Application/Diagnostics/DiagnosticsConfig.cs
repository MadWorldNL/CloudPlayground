using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MadWorldNL.CloudPlayground.Diagnostics;

public class DiagnosticsConfig
{
    public const string ServiceName = "MadWorldNL.CloudPlayground.Api.Web";
    
    public Meter Meter = new Meter(ServiceName);
    public ActivitySource Source = new ActivitySource(ServiceName);
}