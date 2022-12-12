using MyHostAPI.Domain.Premise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Data.Specifications
{
    public class PremiseTypeSpecification
    {
        public class PremiseTypeById : BaseSpecification<PremiseType>
        {
            public PremiseTypeById(string id) : base ( x => x.Id == id)
            {
            }

        }

        public class PremiseTypeByName: BaseSpecification<PremiseType>
        {
            public PremiseTypeByName(string name) : base(x => x.Name == name)
            {
            }

        }

        public class PremiseTypeContainsPremise : BaseSpecification<PremiseType>
        {
            public PremiseTypeContainsPremise(string id) : base (x => x.PremiseIds.Contains(id))
            {
            }
        }
    }
}
