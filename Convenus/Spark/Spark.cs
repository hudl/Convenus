using Nancy;

namespace Convenus.Spark
{
    public class Spark : NancyModule
    {
        public Spark()
        {
            Get["/spark"] = _ =>
                {
                    return View["Spark.html"];
                };
        }
    }
}
