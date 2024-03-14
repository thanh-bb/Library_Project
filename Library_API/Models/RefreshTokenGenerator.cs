using Library_API.Models;
using System.Security.Cryptography;

namespace Authentication.Models
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {

        private readonly LibraryContext _context;

        public RefreshTokenGenerator(LibraryContext learnDb)
        {
            _context = learnDb;


        }

        public string GenerateToken(string username)
        {
            var randomnumber= new byte[32];

            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string _RefreshToken = Convert.ToBase64String(randomnumber);

                var _user = _context.Refreshtokens.FirstOrDefault(e=>e.UserId == username);  
                if(_user != null)
                {
                    _user.RefreshToken1 = _RefreshToken;
                    _context.SaveChanges();
                }
                else
                {
                    Refreshtoken tblRefreshtoken = new Refreshtoken()
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken1 = _RefreshToken,
                        IsActive = true
                    };
                }

                return _RefreshToken;

            }
        }
    }
}
