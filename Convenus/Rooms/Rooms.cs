using Nancy;

namespace Convenus.Rooms
{
    public class Rooms : NancyModule
    {
        public Rooms()
        {
            Get["/rooms/{room}/{roomName}"] = _ =>
                {
                    return View["Room", new {
                        Room = _.room, 
                        RoomName= ((string)_.roomName).Replace('-',' '), 
                        RefreshInterval = Program.Options.RefreshInterval,
                        CompanyName = Program.Options.CompanyName
                            
                    }];
                };
        }
    }
}
