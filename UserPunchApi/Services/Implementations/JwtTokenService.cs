using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserPunchApi.Models;
using UserPunchApi.Services.Interfaces;

namespace UserPunchApi.Services.Implementations
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        // IConfiguration lets us read appsettings.json values like _config["Jwt:Key"]
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user)
        {
            // STEP A — the signing key
            // We take the secret string from appsettings.json, convert it to bytes,
            // and wrap it in SymmetricSecurityKey. "Symmetric" means the same key
            // is used to sign AND verify — only your server holds it.
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // STEP B — signing credentials
            // HmacSha256 is the algorithm. It hashes the token header+payload
            // with the key above. If anyone tampers with the token, the hash won't match.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // STEP C — claims (the payload of the token)
            // Claims are key-value pairs that get embedded inside the token.
            // Anyone can READ them (base64 decoded), but nobody can FORGE them
            // without the secret key. Never put passwords or sensitive data here.
            var claims = new List<Claim>
            {
                // Sub = "subject" — standard JWT field for the user's unique ID
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                // Email embedded so the server can identify the user without a DB lookup
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                // Jti = "JWT ID" — a unique ID for this specific token.
                // Useful later if you want to blacklist individual tokens.
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // ClaimTypes.Role is what [Authorize(Roles = "Manager")] looks for.
                // IMPORTANT: use ClaimTypes.Role (not a plain "role" string) so that
                // ASP.NET's built-in role checks work automatically.
                new Claim(ClaimTypes.Role, user.Role)
            };

            // STEP D — assemble the token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],           // who created this token
                audience: _config["Jwt:Audience"],       // who should accept this token
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_config["Jwt:ExpiryMinutes"]!)),
                signingCredentials: creds
            );

            // STEP E — serialize to the compact "aaa.bbb.ccc" string format
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            // 64 cryptographically random bytes → base64 string.
            // This is NOT a JWT — it's just a secure random string.
            // When you store refresh tokens in the DB later, you'll
            // hash this before saving (same reason you hash passwords).
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
