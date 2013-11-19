using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace Convenus.Setup
{
    public class Setup : NancyModule
    {
        public Setup()
        {
            Get["/setup"] = _ =>
                {
                    return View["setup"];
                };
        }
    }
}
