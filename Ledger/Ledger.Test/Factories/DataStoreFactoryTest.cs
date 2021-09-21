using FluentAssertions;
using Ledger.Enums;
using Ledger.Factories;
using Ledger.Repository;
using Xunit;

namespace Ledger.Test.Factories
{
    public class DataStoreFactoryTest
    {
        [Fact]
        public void GetDataStore_WithInmemoryType_Retuns_InMemoryDataStore()
        {
            var dataStore = DataStoreFactory.GetDataStore(DataStoreType.InMemoryStore);
            dataStore.Should().NotBeNull();
            dataStore.GetType().Should().Be(typeof(InMemoryDataStore));
        }

        [Fact]
        public void GetDataStore_WithDefaultType_Retuns_InMemoryDataStore()
        {
            var dataStore = DataStoreFactory.GetDataStore(0);
            dataStore.Should().NotBeNull();
            dataStore.GetType().Should().Be(typeof(InMemoryDataStore));
        }
    }
}