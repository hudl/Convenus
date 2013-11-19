using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace Convenus
{
    public class LandingPage : NancyModule
    {
        public LandingPage()
        {
            Get["/"] = _ =>
                {
                    return View["LandingPage.html", new { Company = Program.Options.CompanyName ?? "Convenus" }];
                };

        }
    }
}
