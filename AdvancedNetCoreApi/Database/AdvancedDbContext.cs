using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AdvancedNetCoreApi.Database
{
    public class AdvancedDbContext : DbContext
    {
        public AdvancedDbContext(DbContextOptions<AdvancedDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var entityMethod = typeof(ModelBuilder).GetMethod("Entity");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var entityTypes = assembly
                  .GetTypes()
                  .Where(t => t.BaseType!=null && t.BaseType.GetInterface("IEntity") != null);

                foreach (var entityType in entityTypes)
                {
                    var entity = modelBuilder.Entity(entityType.Name);

                    entity
                        .Property<string>("Id")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                    entity
                        .Property<DateTime>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                    entity
                        .Property<DateTime>("UpdatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate();

                    entity
                        .Property<string>("RowVersion")
                        .IsRequired()
                        .IsRowVersion();

                    entity
                        .Property<bool>("IsDeleted")
                        .IsRequired()
                        .HasDefaultValue(false);
                }

                foreach (var type in entityTypes)
                {
                    entityMethod.MakeGenericMethod(type)
                      .Invoke(modelBuilder, new object[] { });
                }
            }
            base.OnModelCreating(modelBuilder);

        }
    }
}
