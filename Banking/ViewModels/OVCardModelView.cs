using Banking.Models;
using Banking.Views;
using System.Collections.Generic;
using System.Linq;

namespace Banking.ModelViews
{
	public class OVCardModelView
	{
		public OptionModelView Options { get; set; }
		public OVCardList ListView { get; set; }
		public List<OVCard> Cards { get; set; }


		public OVCardModelView(OptionModelView options, OVCardList listView)
		{
			Options = options;
			ListView = listView;

			OpenCards();
		}

		private void OpenCards()
		{

			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				var cards = (from a in db.OVCards
										 orderby a.Id descending
										 select a).ToList();
				Cards = new List<OVCard>(cards);
			}

		}
	}
}
