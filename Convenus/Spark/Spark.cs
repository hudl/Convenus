using Nancy;

namespace Convenus.Spark
{
    public class Spark : NancyModule
    {
        public Spark()
        {
            Get["/spark"] = _ =>
                {
                    return View["Spark.html", new
                    {
                        SparkUserName = Program.Options.SparkUserName ?? "",
                        SparkPassword = Program.Options.SparkPassword ?? ""
                    }];
                };
        }
    }
}
