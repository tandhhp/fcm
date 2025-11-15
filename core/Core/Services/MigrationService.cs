using Microsoft.EntityFrameworkCore;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;

namespace Waffle.Core.Services;

public class MigrationService : IMigrationService
{
    private readonly ApplicationDbContext _context;
    public MigrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> MoveComponentsAsync(string currentComponent, string newComponent)
    {
        var block = from a in _context.Components
                    join b in _context.WorkContents on a.Id equals b.ComponentId
                    where a.NormalizedName == currentComponent
                    select b;
        var editor = await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName == newComponent);
        if (editor is null) return false;
        foreach (var item in block)
        {
            item.ComponentId = editor.Id;
        }
        await _context.SaveChangesAsync();
        return true;
    }
}
