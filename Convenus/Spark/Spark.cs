using Nancy;

namespace Convenus.Spark
{
    public class Spark : NancyModule
    {
        public Spark()
        {
            Get["/spark"] = _ =>
                {
                    if (Program.Options.EnableSpark.HasValue && (bool) Program.Options.EnableSpark)
                    {
                        return View["Spark.html"];
                    }
                    return HttpStatusCode.NotFound;
                };
        }
    }
}
