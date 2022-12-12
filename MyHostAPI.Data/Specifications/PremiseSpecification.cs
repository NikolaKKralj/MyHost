using MyHostAPI.Domain.Premise;

namespace MyHostAPI.Data.Specifications
{
    public class PremiseSpecification
    {
        public class PremiseById : BaseSpecification<Premise>
        {
            public PremiseById(string id) : base(b => b.Id == id && b.IsDeleted == false)
            {

            }
        }
        public class PremiseByImage : BaseSpecification<Premise>
        {
            public PremiseByImage(string image) : base(b => b.Images.Any(x => x.Path.Contains(image)))
            {

            }
        }

        public class PremiseByMenuItemImage : BaseSpecification<Premise>
        {
            public PremiseByMenuItemImage(string image) : base(b => b.MenuItems.Any(x => x.Path.Contains(image)))
            {

            }
        }

        public class ActivePremises : BaseSpecification<Premise>
        {
            public ActivePremises() : base(b => b.IsDeleted == false)
            {

            }
        }

        public class PremisesByFavoritePremiseList : BaseSpecification<Premise>
        {
            public PremisesByFavoritePremiseList(List<string> ids) : base(b => ids.Contains(b.Id) && b.IsDeleted == false)
            {

            }
        }
    }
}

