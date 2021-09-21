using Ledger.Enums;
using Ledger.Processors;

namespace Ledger.Factories
{
    public static class ProcessorFactory
    {
        public static IProcessor CreateProcessor(ProcessorType processorType, object args)
        {
            // Add more Processor Types when and if needed if input data source changes
            IProcessor processor = processorType switch
            {
                ProcessorType.FileProcessor => new FileProcessor((string)args),
                _ => new FileProcessor((string)args),
            };
            return processor;
        }
    }
}