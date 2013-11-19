using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Conventions;

namespace Convenus
{
    public class StaticFilesBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            //external nuget loaded js
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts", allowedExtensions: "js"));
            //external nuget loaded css
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", allowedExtensions: "css"));

            //our js
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", allowedExtensions: "js"));
            //our css
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("css", allowedExtensions: "css"));
        }

    }
}
