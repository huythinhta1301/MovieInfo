using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using Movie.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IConfiguration _config;
        public UserController(DataContext context, IConfiguration config)
        {
            _context = context;

            _config = config;
        }
        [HttpPost]
        public IActionResult SendEmail(string body, string emailTo, string emailTitle)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("tahuythinh@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = emailTitle;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("tahuythinh@gmail.com", "krukrcguhhtjuejh");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }

        private string CreateTokenJWT(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("This is my secret key value"));


            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.User.Any(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerficationToken = CreateRandomToken()
            };
            var emailTitle = $"Hello {user.Email}, please verify !";

            var html = $"<form method=\"post\" action=\"https://localhost:7086/api/User/verify?token={user.VerficationToken}\" class=\"inline\">" +
                $"<button type=\"submit\" name=\"submit_param\" value=\"submit_value\" class=\"link-button\">   Verify your account  </button>" +
                $"</form>";
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            SendEmail(html, user.Email, emailTitle);

            System.Diagnostics.Debug.WriteLine(user.Email);
            return Ok(user);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("Not verified");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Password is incorrect.");
            }
            string token = CreateTokenJWT(user);
            // return Ok($"Welcome back, {user.Email} " token);
            return Ok(token);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();
            var html = $"<form method=\"post\" action=\"https://localhost:7086/api/User/forgot-password?token={user.PasswordResetToken}\" class=\"inline\">" +
                $"<button type=\"submit\" name=\"submit_param\" value=\"submit_value\" class=\"link-button\">   Verify your account  </button>" +
                $"</form>";
            var title = "Reset password email";
            SendEmail(html, user.Email, title);
            return Ok("Password token has been send to ur emai !");
        }
        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null)
            {
                return BadRequest("Invalid Token");
            }

            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("User verified !");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("User verified !");
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetListUser()
        {
            var result = await _context.User.ToListAsync();
            return Ok(result);
        }

        

    }
}
