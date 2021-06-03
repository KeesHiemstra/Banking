using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
	{
		private readonly List<string> lineBuffer = new List<string>();
		private readonly List<ListItem> itemBuffer = new List<ListItem>();

		public HistoryWindow()
		{
			InitializeComponent();

			ShowAbout();
			ShowHistory();
		}

		private void ShowAbout()
		{
			ApplicationTitleTextBlock.Text = NameValue("",
				Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
			VersionTextBlock.Text = NameValue("Version: ",
				Assembly.GetExecutingAssembly().GetName().Version.ToString());
			DiscriptionTextBlock.Text = NameValue("",
				Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description);
		}

		private string NameValue(string Name, string Value)
		{
			string result = string.Empty;
			if (!string.IsNullOrEmpty(Value))
			{
				result = $"{Name}{Value}";
			}
			return result;
		}

		private void ShowHistory()
		{
			string[] history = Banking.Properties.Resources.History
				.Replace("\r\n", "\n")
				.Split('\n');
			bool IsInList = false;

			foreach (string line in history)
			{
				if (line.ToLower().StartsWith("version"))
				{
					if (lineBuffer.Count > 0)
					{
						AddParagraph();
					}
					if (itemBuffer.Count > 0)
					{
						BufferToList();
					}

					HistoryFlowDocument.Blocks.Add(new Paragraph(new Run(line))
					{
						FontSize = 14,
						FontWeight = FontWeights.Bold
					});
				}
				else if (line.StartsWith("* "))
				{
					if (lineBuffer.Count > 0)
					{
						if (IsInList)
						{
							itemBuffer.Add(new ListItem(new Paragraph(new Run(BufferToString()))));
						}
						else
						{
							HistoryFlowDocument.Blocks.Add(new Paragraph(new Run(BufferToString())));
						}
						lineBuffer.Clear();
					}

					IsInList = true;
					lineBuffer.Add(line.Remove(0, 2));
				}
				else if (string.IsNullOrWhiteSpace(line))
				{
					if (IsInList)
					{
						BufferToList();
					}
					IsInList = false;
				}
				else if (IsInList)
				{
					lineBuffer.Add(line);
				}
				else
				{
					if (lineBuffer.Count > 0)
					{
						AddParagraph();
					}

					lineBuffer.Add(line);
				}
			}

			if (itemBuffer.Count > 0)
			{
				BufferToList();
			}
			if (lineBuffer.Count > 0)
			{
				AddParagraph();
			}
		}

		private string BufferToString()
		{
			string result = string.Empty;

			foreach (string line in lineBuffer)
			{
				result = $"{result} {line.Trim()}".Trim();
			}
			lineBuffer.Clear();
			result = result.Replace("  ", " ");

			return result;
		}

		private void BufferToList()
		{
			if (lineBuffer.Count > 0)
			{
				itemBuffer.Add(new ListItem(new Paragraph(new Run(BufferToString()))
				{ TextAlignment = TextAlignment.Left }));
			}

			List list = new List() { TextAlignment = TextAlignment.Left };
			foreach (ListItem item in itemBuffer)
			{
				list.ListItems.Add(item);
			}
			HistoryFlowDocument.Blocks.Add(list);
			itemBuffer.Clear();
		}

		private void AddParagraph()
		{
			var par = BufferToString();
			HistoryFlowDocument.Blocks.Add(
				new Paragraph(
					new Run(par))
				{
					TextAlignment = TextAlignment.Left
				});
		}

	}
}
