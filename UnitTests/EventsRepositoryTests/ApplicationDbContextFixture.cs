using EventsService.Infrastructure.MSSQL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests.EventsRepositoryTests
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public ApplicationDbContext Context { get; }

        public ApplicationDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    [CollectionDefinition("ApplicationDbContextCollection")]
    public class ApplicationDbContextCollection : ICollectionFixture<ApplicationDbContextFixture>
    {
    }
}
