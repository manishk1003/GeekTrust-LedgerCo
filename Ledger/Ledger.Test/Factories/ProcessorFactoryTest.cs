using System;
using FluentAssertions;
using Ledger.Factories;
using Ledger.Processors;
using Ledger.Resources;
using Moq;
using Xunit;

namespace Ledger.Test.Factories
{
    public class ProcessorFactoryTest
    {

        [Fact]
        public void CreateProcessor_WithFileTypeAndNullArgs_Throws_Exception()
        {
            Action act = () => ProcessorFactory.CreateProcessor(Enums.ProcessorType.FileProcessor, It.IsNotNull<string>());
            act.Should().Throw<ArgumentException>().WithMessage(ErrorMessages.FilePathError);
           
        }

        [Fact]
        public void CreateProcessor_WithFileType_Retuns_FileProcessor()
        {
            string filePath = "DummyFileName.txt";
            var processor = ProcessorFactory.CreateProcessor(Enums.ProcessorType.FileProcessor, filePath);
            processor.Should().NotBeNull();
            processor.GetType().Should().Be(typeof(FileProcessor));
        }

        [Fact]
        public void CreateProcessor_WithDefaultType_Retuns_FileProcessor()
        {
            string filePath = "DummyFileName.txt";
            var processor = ProcessorFactory.CreateProcessor(0, filePath);
            processor.Should().NotBeNull();
            processor.GetType().Should().Be(typeof(FileProcessor));
        }
    }
}