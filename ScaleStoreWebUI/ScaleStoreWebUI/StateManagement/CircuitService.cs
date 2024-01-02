namespace ScaleStoreWebUI.StateManagement;

public class CircuitService
{
    public string CircuitId { get; private set; }

    public void Initialize(string circuitId)
    {
        CircuitId = circuitId;
    }
}