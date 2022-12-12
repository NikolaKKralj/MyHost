using MyHostAPI.Common.Helpers;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Premise;
using static MyHostAPI.Data.Specifications.PremiseTypeSpecification;

namespace MyHostAPI.Data.Seeder
{
    public class Seeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IPremiseTypeRepository _premiseTypeRepository;

        public Seeder(IUserRepository userRepository, IPremiseTypeRepository premiseTypeRepository)
        {
            _userRepository = userRepository;
            _premiseTypeRepository = premiseTypeRepository;
        }

        public async Task SeedAsync()
        {

            #region Admin
            var existingAdmin = await _userRepository.FindOneByAsync(new UserByUsername("admin@mail.com"));

            if (existingAdmin == null)
            {
                User admin = new User()
                {
                    Name = "Admin",
                    Identity = new()
                    {
                        Email = "admin@mail.com",
                        Password = PasswordHandler.HashPassword("Sifra.123"),
                        Role = Role.Admin
                    }
                };

                await _userRepository.CreateAsync(admin);
            }
            #endregion

            #region PremiseType

            var listOfPremiseTypes = new List<string>() { "Restaurant", "Pub", "PastryShop", "Fastfood", "CoffeeShop", "NightClub" };

            foreach (var nameType in listOfPremiseTypes) 
            {
                var existingPremiseType = await _premiseTypeRepository.FindOneByAsync(new PremiseTypeByName(nameType));

                if (existingPremiseType == null)
                {
                    PremiseType premiseType = new PremiseType() { Name = nameType };

                    await _premiseTypeRepository.CreateAsync(premiseType);
                }
            }

            #endregion
        }
    }
}
