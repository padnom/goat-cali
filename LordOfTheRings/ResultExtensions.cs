using LordOfTheRings;

public static class ResultExtensions
{
    public static void HandleResult(this Result result)
    {
        if (!result.IsSuccess)
        {
            Console.WriteLine($"Operation failed: {result.Error}");
        }
    }
}