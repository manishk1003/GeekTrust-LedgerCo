using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Factories;
using Ledger.Handlers;
using Ledger.Helpers;
using Ledger.Resources;
using Ledger.Response;

namespace Ledger.Processors
{
    public class FileProcessor : IProcessor
    {
        private readonly string _filePath;

        public FileProcessor(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException(ErrorMessages.FilePathError);
            }
            _filePath = filepath;
        }

        public async Task ProcessAsync()
        {
            var commands = GetCommands();
            foreach (string commandParams in commands)
            {
                var requestHandler = RequestHandlerFactory.GetRequestHandler(commandParams);
                if (requestHandler != null)
                {
                    var response = await requestHandler.HandleAsync();
                    if (requestHandler.GetType() == typeof(BalanceHandler) && response.Success)
                    {
                        var balanceResponse = (BalanceResponse)response;
                        Utils.ConsoleLogSuccess($"{balanceResponse.BankName} {balanceResponse.BorrowerName} {balanceResponse.AmountPaid} {balanceResponse.RemainingEmis}");
                    }
                }
                else
                {
                    Utils.ConsoleLogError(ErrorMessages.InvalidCommand);
                }
            }
        }

        private IEnumerable<string> GetCommands()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullFilePath = Path.Combine(basePath, Constants.InputSourceFolder, _filePath);
           
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException(ErrorMessages.FileNotFound);
            }

            IEnumerable<string> fileCommands = File.ReadAllLines(fullFilePath);
            if (fileCommands == null || !fileCommands.Any())
            {
                throw new ArgumentException(ErrorMessages.CommandNotFound);
            }
            return fileCommands;
        }
    }
}