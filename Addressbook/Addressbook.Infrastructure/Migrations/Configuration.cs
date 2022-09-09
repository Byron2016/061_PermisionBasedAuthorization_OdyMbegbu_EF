namespace Addressbook.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Addressbook.Infrastructure.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Addressbook.Infrastructure.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Users.AddOrUpdate(u => u.Email, new Entities.User
            {
                Email = "admin@gmail.com",
                Password = "5f4dcc3b5aa765d61d8327deb882cf99".ToUpper()
            });
        }
    }
}
