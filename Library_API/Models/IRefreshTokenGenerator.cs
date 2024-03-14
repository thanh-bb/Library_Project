namespace Library_API.Models
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken(string username);


    }
}
