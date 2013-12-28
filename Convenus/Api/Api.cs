﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Helpers;

namespace Convenus.Api
{

    public class Api:NancyModule
    {
        private const string AuthTokenKey = "auth-token";

        public Api() :base("/api")
        {
            //get roomlists (building, region)
            //VERY CACHEABLE
            Get["/roomlists"] = _ =>
                {
                    return Response.AsJson(ExchangeServiceHelper.GetRoomLists());
                };

            //get the rooms for a room list
            //CACHEABLE
            Get["/roomlists/{roomlist}/rooms"] = _ =>
                {
                    return Response.AsJson(ExchangeServiceHelper.GetRooms((string)_.roomlist));
                };

            //generate auth info for a given room
            Post["/rooms/{room}/auth"] = _ =>
                {
                    //if auth turned off - return nothing
                    if (!Program.Options.RequireAuth.GetValueOrDefault(false))
                    {
                        return HttpStatusCode.OK;
                    }
                    var auth = CreateHash(Request.Form["pin"], _.room);

                    return Response.AsText("OK").AddCookie(AuthTokenKey, auth);
                };

            //get a rooms status
            //SOMEWHAT CACHEABLE
            Get["/rooms/{room}"] = _ =>
                {
                    //if auth is enabled - check for the room
                    if (Program.Options.RequireAuth.GetValueOrDefault(false) && !CheckAuth((string)_.room,Request.Cookies))
                    {
                        //NOTE: this isn't 'real' security - it won't stop people determined to use this. but seriously who is going to 
                        //try that hard for meeting room software on an intranet
                        return HttpStatusCode.Forbidden;
                    }
                    return Response.AsJson(new {
                        id=(string)_.room,
                        Events=ExchangeServiceHelper.GetRoomAvailability((string)_.room)
                    });
                };

            Post["/rooms/{room}/reservation"] = _ =>
            {
                //if auth is enabled - check for the room
                if (Program.Options.RequireAuth.GetValueOrDefault(false) && !CheckAuth((string)_.room, Request.Cookies))
                {
                    //NOTE: this isn't 'real' security - it won't stop people determined to use this. but seriously who is going to 
                    //try that hard for meeting room software on an intranet
                    return HttpStatusCode.Forbidden;
                }

                ExchangeServiceHelper.CreateReservation(_.room, int.Parse(Request.Form.duration));

                return Response.AsText("").WithStatusCode(HttpStatusCode.Created);

            };

            Get["/roomlist/{roomlist}/availability"] = _ =>
            {
                var roomList = (string)_.roomlist;
                //24hr time of room to request
                //if null - then now
                var time = (DynamicDictionaryValue)Request.Query["time"];
                if (!time.HasValue)
                {
                    time = new DynamicDictionaryValue(DateTime.Now.ToString("HH:mm:00"));  
                }

                //in minutes
                //defaults to 30 min
                var duration = (DynamicDictionaryValue)Request.Query["duration"];
                if (!duration.HasValue)
                {
                    duration = new DynamicDictionaryValue("30");
                }

                var requestStartTime = DateTime.Parse(time);
                var requestEndTime = requestStartTime.AddMinutes(int.Parse(duration));

                var availableRooms = ExchangeServiceHelper.GetAvailabileRoomsByRoomList(roomList, requestStartTime, requestEndTime);

                return Response.AsJson(availableRooms);
            };
        }

        private static bool CheckAuth(string room, IDictionary<string,string> cookies)
        {
            if (!cookies.ContainsKey(AuthTokenKey)) return false;

            var authToken = HttpUtility.UrlDecode(cookies[AuthTokenKey]); //url decode the equal sign from base64
            //calc the correct token
            //note: authpin will always have a value since it has a default
            Debug.Assert(Program.Options.AuthPin != null, "Program.Options.AuthPin != null");
            string answer = CreateHash(Program.Options.AuthPin.Value.ToString(), room);

            return authToken.Equals(answer);
        }
        private static string CreateHash(string pin, string room)
        {
            var stringBytes = Encoding.ASCII.GetBytes(string.Concat(pin, room));
            return Convert.ToBase64String(SHA1.Create().ComputeHash(stringBytes));
        }
    }
}
