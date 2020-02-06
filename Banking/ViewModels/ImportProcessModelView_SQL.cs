using CHi.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Banking.ModelViews
{
	public partial class ImportProcessModelView
	{
		public Dictionary<string, string> ImportToBank { get; private set; } = 
			new Dictionary<string, string>();
		public Dictionary<string, string> MissedTallies { get; private set; } =
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

		private void DictMissedTallies()
		{

			string MissedTalliesJson = "%OneDrive%\\Data\\Banking\\MissedTallies.json".TranslatePath();
			if (File.Exists(MissedTalliesJson))
			{
				using (StreamReader stream = File.OpenText(MissedTalliesJson))
				{
					string json = stream.ReadToEnd();
					MissedTallies = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
				}
			}

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
