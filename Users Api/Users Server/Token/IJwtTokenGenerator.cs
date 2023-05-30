namespace Users_Server.Token
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}