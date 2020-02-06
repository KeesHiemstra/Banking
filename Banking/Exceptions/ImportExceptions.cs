using System;
using System.Runtime.Serialization;

namespace Banking.Exceptions
{

	[Serializable]
	public class ImportException : Exception
	{
		public ImportException() { }
		public ImportException(string message) : base(message) { }
		public ImportException(string message, Exception inner) : base(message, inner) { }
		protected ImportException(
		SerializationInfo info,
		StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class ImportFileException : Exception
	{
		private static string GenerateErrorMessage(string fileName) =>
			$"The import file \"{fileName}\" doesn't exist.";
		public string ErrorMessage { get; }

		public ImportFileException() { }

		public ImportFileException(string message) :
			base(GenerateErrorMessage(message))
		{
			ErrorMessage = message;
		}

		public ImportFileException(string message, Exception inner) :
			base(GenerateErrorMessage(message), inner)
		{ }

		protected ImportFileException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{ }
	}


	[Serializable]
	public class ImportFileHeaderException : Exception
	{
		private static string GenerateErrorMessage(string fileName, int numberOfFields, int countOfFields) =>
	$"The import file \"{fileName}\" expected {numberOfFields} fields, not {countOfFields}.";
		public string FileName { get; }
		public int NumberOfFields { get; }
		public int CountOfFields { get; }

		public ImportFileHeaderException() { }

		public ImportFileHeaderException(string fileName, int numberOfFields, int countOfFields) :
			base(GenerateErrorMessage(fileName, numberOfFields, countOfFields))
		{
			FileName = fileName;
			NumberOfFields = numberOfFields;
			CountOfFields = countOfFields;
		}

		public ImportFileHeaderException(string fileName, int numberOfFields, int countOfFields, Exception inner) :
			base(GenerateErrorMessage(fileName, numberOfFields, countOfFields), inner) { }

		protected ImportFileHeaderException(
		SerializationInfo info,
		StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class ImportStreamException : Exception
	{
		private static string GenerateErrorMessage(string errorMessage) =>
			$"The import stream results in an error:\n{errorMessage}.";
		public string ErrorMessage { get; }

		public ImportStreamException() { }

		public ImportStreamException(string errorMessage) :
			base(GenerateErrorMessage(errorMessage))
		{
			ErrorMessage = errorMessage;
		}

		public ImportStreamException(string errorMessage, Exception inner) :
			base(GenerateErrorMessage(errorMessage), inner)
		{ }

		protected ImportStreamException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{ }
	}

	[Serializable]
	public class ImportDateException : Exception
	{
		private static string GenerateErrorMessage(string errorDate) =>
			$"\"{errorDate}\" is invalid.";
		public string ErrorDate { get; }

		public ImportDateException() { }

		public ImportDateException(string errorDate) :
			base(GenerateErrorMessage(errorDate))
		{
			this.ErrorDate = errorDate;
		}

		public ImportDateException(string errorDate, Exception innerException) :
			base(GenerateErrorMessage(errorDate), innerException)
		{ }

		protected ImportDateException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{ }

	}
}
