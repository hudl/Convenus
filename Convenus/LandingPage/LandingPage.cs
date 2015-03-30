using Nancy;

namespace Convenus.LandingPage
{
    public class LandingPage : NancyModule
    {
        public LandingPage()
        {
            Get["/"] = _ =>
                {
                    return View["LandingPage.html", new
                    {
                        Company = Program.Options.CompanyName ?? "Convenus",
                        EnableSpark = Program.Options.EnableSpark ?? false
                    }];
                };

        }
    }
}
