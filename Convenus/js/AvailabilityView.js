var AvailabilityView = Backbone.Marionette.ItemView.extend({
    template: '#availability-template',
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {
        this.render();
    },
    serializeData: function () {
        var desc, state;

        var currentMeeting = this.model.getCurrentMeeting();
        if (currentMeeting != null) {
            //in currently meeting
            desc = '<h1>Occupied</h1>';
            state = '<h2>' + 'until ' + currentMeeting.EndTime.format('h:mm a') + '</h4>'; //retrieve from current meeting item
        } else {
            desc = '<h1>Available</h1>';
            var nextMeeting = this.model.getNextMeeting();
            if (nextMeeting != null) {
                state = '<h2>' + 'until ' + nextMeeting.StartTime.format('h:mm a') + '</h4>';//retrieve start time of next meet
            } else {
                state = '<h2>for the rest of the day</h4>';
            }
        }

        return {
            IsAvailableDescription: desc,
            StateUntilTime: state
        };
    }

});