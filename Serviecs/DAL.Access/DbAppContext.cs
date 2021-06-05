using DAL.Access;

namespace Serviecs.DAL.Access
{
    public class DbAppContext
    {
       public ContactRepository Contacts { get; }

        public DbAppContext()
        {
            Contacts = new ContactRepository();
        }
    }
}
