using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace Convenus
{
    public static class ExchangeServiceHelper
    {
        private static ExchangeService _exchangeService;
        private static MemoryCache _memoryCache;

        public static void Init(string userName, SecureString password)
        {
            _exchangeService = new ExchangeService(TimeZoneInfo.Local);
            _memoryCache = new MemoryCache("ExchangeCache");
            //exchangeService.TraceEnabled = true;
            //exchangeService.TraceEnablePrettyPrinting = true;

            _exchangeService.Credentials = new NetworkCredential(userName, password);
            _exchangeService.AutodiscoverUrl(userName, url => true);
        }

        public static List<RoomList> GetRoomLists()
        {
            const string RoomListKey = "room-list-key";
            var returnValue = (List<RoomList>)_memoryCache.Get(RoomListKey);
            if (returnValue == null)
            {
                var roomLists = _exchangeService.GetRoomLists();
                returnValue = roomLists.Select(r => new RoomList()
                {
                    Address = r.Address,
                    Name = r.Name
                }).OrderBy(rl => rl.Name).ToList();

                //this barely changes in an organization - so cache for a while
                _memoryCache.Add(RoomListKey, returnValue, DateTime.Now.AddHours(1));
            }

            return returnValue;
        }

        public static List<Room> GetRooms(string roomListAddress)
        {
            string RoomsKey = string.Format("rooms-key-{0}", roomListAddress);
            var returnValue = (List<Room>)_memoryCache.Get(RoomsKey);
            if (returnValue == null)
            {
                var rooms = _exchangeService.GetRooms(roomListAddress);
                returnValue = rooms.Select(r => new Room()
                {
                    Name = r.Name,
                    Address = r.Address
                }).OrderBy(r => r.Name).ToList();

                //this barely changes in an organization - so cache for a while
                _memoryCache.Add(RoomsKey, returnValue, DateTime.Now.AddHours(1));
            }

            return returnValue;
        }
        public static List<CalendarEvent> GetRoomAvailability(string roomAddress)
        {
            //short cache just to prevent crazy accesses
            //it is recommended to keep this cache shorter than the page refresh 
            //otherwise the frequent refresh doesn't do any good
            string RoomEventsKey = GetRoomEventsKey(roomAddress);
            var returnValue = (List<CalendarEvent>)_memoryCache.Get(RoomEventsKey);
            if (returnValue == null)
            {
                var attendee = new AttendeeInfo(roomAddress, MeetingAttendeeType.Room, false);
                var results = _exchangeService.GetUserAvailability(new List<AttendeeInfo>() { attendee },
                                                    new TimeWindow(DateTime.Today, DateTime.Today.AddDays(1)),
                                                    AvailabilityData.FreeBusy);
                returnValue = results.AttendeesAvailability.SelectMany(a => a.CalendarEvents).Select(e => new CalendarEvent()
                {
                    Subject = e.Details.Subject,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Status = e.FreeBusyStatus.ToString()

                }).OrderBy(e => e.StartTime).ToList();

                //add the pending item if necessary
                var pendingItem = (CalendarEvent)_memoryCache.Get(GetRoomPendingEventKey(roomAddress));

                //match on StartTime/EndTime because we know its unique for a given room
                if (pendingItem != null && !returnValue.Any(c => c.EndTime.Equals(pendingItem.EndTime) && c.StartTime.Equals(pendingItem.StartTime)))
                {
                    returnValue.Add(pendingItem);
                }

                _memoryCache.Add(RoomEventsKey, returnValue, DateTime.Now.AddSeconds(30));
            }

            return returnValue;

            //if we need attendee details in the future - use this
//            var appointments = exchangeService.FindAppointments(new FolderId(WellKnownFolderName.Calendar,new Mailbox(roomAddress)), 
//                                             new CalendarView(DateTime.Today, DateTime.Today.AddDays(4)));


        }

        public static void CreateReservation(string roomAddress, int duration)
        {

            Appointment newApt = new Appointment(_exchangeService);
            newApt.Subject = "Walk up";
            newApt.Start = DateTime.Now;
            newApt.StartTimeZone = TimeZoneInfo.Local;
            newApt.End = DateTime.Now.AddMinutes(duration);
            newApt.EndTimeZone = TimeZoneInfo.Local;
            newApt.Resources.Add(roomAddress);

            newApt.Save(SendInvitationsMode.SendOnlyToAll);

            //set 'pending' item so that quick GetRoomAvailability calls
            //won't miss this appointment just because the meeting room
            //hasn't accepted yet. Since this only applies to  Walk Up
            //reservations (and you can only have one of those at a time)
            //we can store this in a single cached value
            _memoryCache.Add(GetRoomPendingEventKey(roomAddress), 
                new CalendarEvent()
                {
                    Subject = newApt.Subject,
                    StartTime = newApt.Start,
                    EndTime = newApt.End,
                    Status = "Busy" 
                }, 
                DateTime.Now.AddMinutes(2));

            _memoryCache.Remove(GetRoomEventsKey(roomAddress));

        }

        private static string GetRoomEventsKey(string roomAddress)
        {
            return string.Format("room-events-key-{0}", roomAddress);
        }
        private static string GetRoomPendingEventKey(string roomAddress)
        {
            return string.Format("room-pending-key-{0}", roomAddress);
        }
    }

    public class RoomList
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class Room
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class CalendarEvent
    {
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }

    }
}
