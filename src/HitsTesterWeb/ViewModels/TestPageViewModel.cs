using System.Collections.Generic;

namespace HitsTesterWeb.ViewModels
{
	public class TestPageViewModel
	{
		public TestPageViewModel(string text, string translatedText, List<string> hits)
		{
			Text = text;
			TranslatedText = translatedText;
			Hits = hits;
		}

		public string Text { get; }
		public string TranslatedText { get; }
		
		public List<string> Hits { get; }
	}
}