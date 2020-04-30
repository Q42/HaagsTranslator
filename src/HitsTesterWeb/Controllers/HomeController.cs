using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HitsTesterWeb.Models;
using HitsTesterWeb.ViewModels;

namespace HitsTesterWeb.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		[Route("/")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("/")]
		public IActionResult IndexPost(FormModel model)
		{
			var hits = HaagsTranslator.Translator.GetHits(model.Text).Where(h => h.Item2).Select(t => t.Item1).ToList();

			return View("Index", new TestPageViewModel(model.Text, HaagsTranslator.Translator.Translate(model.Text), hits));
		}
	}
}