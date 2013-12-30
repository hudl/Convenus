var RoomModel = Backbone.Model.extend({
    urlRoot: '/api/rooms/',
    defaults: {
        id: '',
        Events: [],
        RoomList: null,
        AvailableRooms: []
    },
    parse: function (attributes) {
        _.each(attributes.Events, function (event) {
            event.StartTime = moment(event.StartTime);
            event.EndTime = moment(event.EndTime);
        });
        return attributes;
    },
    getCurrentMeeting: function () {
        var curTime = moment();
        var curEvent;
        _.each(this.get('Events'), function (event) {
            if (curEvent == null && event.StartTime.isBefore(curTime) && event.EndTime.isAfter(curTime)) {
                curEvent = event;
            }
        });
        return curEvent;
    },
    getNextMeeting: function () {
        var curTime = moment();
        var curEvent;
        _.each(this.get('Events'), function (event) {
            if (curEvent == null && event.StartTime.isAfter(curTime)) {
                curEvent = event;
            }
        });
        return curEvent;
    }
});