var ReservationAreaView = Backbone.Marionette.ItemView.extend({
    template: '#reservation-area-template',
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {
        this.render();
    },
    serializeData: function () {
        var hasCurrentMeeting, curMeetingName, nextMeetingWithin15, nextMeetingWithin30, nextMeetingWithin60;

        var currentMeeting = this.model.getCurrentMeeting();
        if (currentMeeting != null) {
            hasCurrentMeeting = true;
            curMeetingName = currentMeeting.Subject;

        } else {
            var nextMeeting = this.model.getNextMeeting();
            
            hasCurrentMeeting = false;
            if (nextMeeting) {
                //warning you can't cache momemt() into a local variable because 'add' overwrites it
                nextMeetingWithin15 = moment().add('minutes', 15).isAfter(nextMeeting.StartTime);
                nextMeetingWithin30 = moment().add('minutes', 30).isAfter(nextMeeting.StartTime);
                nextMeetingWithin60 = moment().add('minutes', 60).isAfter(nextMeeting.StartTime);
            }

        }

        return {
            HasCurrentMeeting: hasCurrentMeeting,
            CurrentMeetingName: curMeetingName,
            HasNextMeetingWithin15Min: nextMeetingWithin15,
            HasNextMeetingWithin30Min: nextMeetingWithin30,
            HasNextMeetingWithin60Min: nextMeetingWithin60
        };
    }

});