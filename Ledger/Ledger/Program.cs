using System;
using System.Threading.Tasks;
using Ledger.Enums;
using Ledger.Factories;
using Ledger.Helpers;
using Ledger.Processors;
using Ledger.Resources;

namespace Ledger
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Utils.ConsoleLogError(ErrorMessages.ProvideInput);
                }
                else
                {
                    ProcessCommands(args).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Utils.ConsoleLogError(ex.Message);
            }
        }

        private static async Task ProcessCommands(string[] args)
        {
            IProcessor processor = ProcessorFactory.CreateProcessor(ProcessorType.FileProcessor, args[0]);
            await processor.ProcessAsync();
        }
    }
}