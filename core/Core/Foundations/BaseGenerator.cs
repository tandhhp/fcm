using Waffle.Data;

namespace Waffle.Core.Foundations;

public interface IGenerator
{
    Task RunAsync();
}

public abstract class BaseGenerator : IGenerator
{
    protected readonly ApplicationDbContext _context;
    public BaseGenerator(ApplicationDbContext context)
    {
        _context = context;
    }

    public abstract Task RunAsync();
}
