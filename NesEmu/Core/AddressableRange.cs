namespace NesEmu.Core;

public class AddressableRange
{
    public ushort Minimum { get; set; }
    public ushort Maximum { get; set; }

    public AddressableRange(ushort minimum, ushort maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public bool ContainsAddress(ushort address) => address > Minimum && address < Maximum;
}