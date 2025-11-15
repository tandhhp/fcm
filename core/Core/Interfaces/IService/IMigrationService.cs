namespace Waffle.Core.Interfaces.IService
{
    public interface IMigrationService
    {
        Task<bool> MoveComponentsAsync(string currentComponent, string newComponent);
    }
}
