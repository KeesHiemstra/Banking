﻿using System.Collections.Generic;

namespace Banking.ViewModels
{
	public partial class ImportProcessViewModel
	{
		public Dictionary<string, string> ImportToBank { get; private set; } = 
			new Dictionary<string, string>();

		public Dictionary<string, string> PostImport { get; private set; } =
			new Dictionary<string, string>();

		private void DictImportToBank()
		{
			ImportToBank.Add("Import to Bank", @"
INSERT INTO Bank(I.[Account], I.[Date], I.[Mutation], I.[Amount], I.[Name], I.[CounterAccount], I.[Text], I.[RawText])
SELECT I.[Account], I.[Date], I.[Mutation], I.[Amount], I.[Name], I.[CounterAccount], I.[Text], I.[RawText]
FROM Import AS I
	LEFT JOIN Bank AS B
		ON I.[Account] = B.[Account]
			AND I.[Date] = B.[Date]
			AND I.[Amount] = B.[Amount]
			AND I.[RawText] = B.[RawText]
WHERE B.[Id] IS NULL
ORDER BY I.[Date], I.[Account];
");
			ImportToBank.Add("Month", @"
UPDATE Bank
SET [Month] = CAST(YEAR([Date]) AS varchar(4)) + '-' + RIGHT('0' + CAST(MONTH([Date]) AS varchar(2)), 2)
FROM Bank
WHERE [Month] IS NULL
");
		}

		private void DictPostImport()
		{

			PostImport.Add("Post import", @"
UPDATE Bank
SET [TallyDescription] = T.[Description]
FROM Bank AS B
	LEFT JOIN Tally AS T
		ON B.[TallyName] = T.[Tally]
WHERE B.[TallyDescription] IS NULL
	OR B.[TallyDescription] != T.[Description]
");

		}

	}
}
