using System.Threading.Tasks;

public interface IUserRepository
{
    Task SaveUserAsync(UserData user);
}