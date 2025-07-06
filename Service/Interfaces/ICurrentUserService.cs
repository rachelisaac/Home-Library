namespace Service.Interfaces
{
    public interface ICurrentUserService
    {
        int? GetUserId();
        bool IsAdmin();
    }

}
