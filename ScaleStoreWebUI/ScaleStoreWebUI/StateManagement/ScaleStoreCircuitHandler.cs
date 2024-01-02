using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Collections.Concurrent;

namespace ScaleStoreWebUI.StateManagement;

public class ScaleStoreCircuitHandler : CircuitHandler
{

    public ConcurrentDictionary<string, Circuit> Circuits
    {
        get;
        set;
    }


    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    }

}