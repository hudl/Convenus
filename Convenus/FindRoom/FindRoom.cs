using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;

namespace Convenus
{
    public class FindRoom : NancyModule
    {
        public FindRoom()
        {
            Get["/findroom"] = _ =>
                {
                    return View["FindRoom.html", new { Company = Program.Options.CompanyName ?? "Convenus" }];
                };

        }
    }
}
