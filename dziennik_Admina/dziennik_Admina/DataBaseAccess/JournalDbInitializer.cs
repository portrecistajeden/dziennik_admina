using dziennik_Admina.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.DataBaseAccess
{
    public class JournalDbInitializer : DropCreateDatabaseAlways<JournalDbContext>
    {
        protected override void Seed(JournalDbContext context)
        {
            User admin = new User()
            {
                ID_User=1,
                Username="ADMIN",
                Password= "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", //password: admin - encrypted using SHA256
            };
            context.Users.Add(admin);
            base.Seed(context);
        }
    }
}
