using Ledger.Enums;
using Ledger.Repository;

namespace Ledger.Factories
{
    public static class DataStoreFactory
    {
        public static IDataStore GetDataStore(DataStoreType dataStoreType)
        {
            IDataStore dataStore = null;
            dataStore = dataStoreType switch
            {
                DataStoreType.InMemoryStore => new InMemoryDataStore(),
                _ => new InMemoryDataStore()
            };
            return dataStore;
        }
    }
}