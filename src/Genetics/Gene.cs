namespace UltimateTicTacToe.Genetics;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class Gene : Attribute
{
    public IComparable? Minimum { get; }
    public IComparable? Maximum { get; }
    public Type? RangeType { get; }
    public Gene()
    {
        Minimum = null;
        Maximum = null;
        RangeType = null;
    }
    public Gene(int minimum, int maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        RangeType = typeof(int);
    }
    public Gene(float minimum, float maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        RangeType = typeof(float);
    }
    public Gene(double minimum, double maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        RangeType = typeof(double);
    }
    public bool IsValid(object? value)
    {
        if (RangeType is null)
            return true;
        if (!RangeType.IsInstanceOfType(value))
            return false;
        if (Minimum is null || Maximum is null)
            return true;
        return Minimum.CompareTo(value) <= 0 && 0 <= Maximum.CompareTo(value);
    }
}