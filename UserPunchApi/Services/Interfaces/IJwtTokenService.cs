using UserPunchApi.Models;

namespace UserPunchApi.Services.Interfaces
{
    // Responsible for one thing only: creating tokens.
    // Keeping it separate from AuthService means you can swap the
    // token strategy later (e.g. switch to RS256) without touching login logic.
    public interface IJwtTokenService
    {
        // Takes the user so it can embed their id, email, and role into the token.
        string GenerateAccessToken(User user);

        // A random, opaque string — not a JWT.
        // Its only job is to be hard to guess. The real validation
        // happens when you store it in the DB (future improvement).
        string GenerateRefreshToken();
    }
}
