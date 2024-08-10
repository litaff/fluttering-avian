namespace fluttering_avian_tests.mock;

public class MockRandom : Random
{
    public bool ReturnsUpperBound { get; set; }
    
    /// <summary>
    /// Returns <see cref="minValue"/> or <see cref="maxValue"/> - 1 if <see cref="ReturnsUpperBound"/> is true.
    /// </summary>
    public override int Next(int minValue, int maxValue)
    {
        return ReturnsUpperBound ? maxValue - 1 : minValue;
    }
}