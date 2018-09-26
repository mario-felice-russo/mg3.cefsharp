using Models;

namespace Repositories
{
    public class ContactRepository : Repository<Contact>
    {
        public ContactRepository() : base()
        {
        }

        public override bool VerifyEntity()
        {
            return !string.IsNullOrEmpty(Entity.Name) && !string.IsNullOrEmpty(Entity.Email);
        }

        public override bool VerifyExistencePrimaryKey()
        {
            return Entity.Id > 0;
        }
    }
}
