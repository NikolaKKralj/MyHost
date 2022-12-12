using MyHostAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHostAPI.Data.Specifications
{
    public class TagSpecification
    {
        public class TagByName : BaseSpecification<Tag>
        {
            public TagByName(string name) : base ( x => x.Name == name)
            {
            }
        }
        public class TagByPremiseId : BaseSpecification<Tag>
        {
            public TagByPremiseId(string id) : base(x => x.PremiseIds.Any( x => x.Equals(id)))
            {
            }
        }

        public class ActiveTag : BaseSpecification<Tag>
        {
            public ActiveTag() : base(x => x.IsDeleted == false)
            {

            }
        }
    }
}
