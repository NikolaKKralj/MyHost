using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain.Premise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Data.Repositories
{
    public class PremiseTypeRepository : GenericRepository<PremiseType>, IPremiseTypeRepository
    {
        public PremiseTypeRepository(IMongoClient mongoClient, IConfiguration configuration) 
            : base(mongoClient, configuration)
        {
        }
    }
}
