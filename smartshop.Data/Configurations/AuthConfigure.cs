using Microsoft.Extensions.Configuration;

namespace smartshop.Data.Configurations
{
    public class AuthConfigure
    {
        public AuthConfigure()
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            ConnectionString = root.GetConnectionString("DefaultConnection");

            Token = root.GetSection("AuthConfigures:Token").Value;
            Issuer = root.GetSection("AuthConfigures:Issuer").Value;
            Audience = root.GetSection("AuthConfigures:Audience").Value;
        }

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Token { get; set; }
        public string ConnectionString { get; set; }
    }
}
