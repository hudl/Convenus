var AvailabilityView = Backbone.Marionette.ItemView.extend({
    template: '#room-availability-template',
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {
        this.render();
    },
    onRender: function () {
        this.$el.removeClass('unavailable available').addClass((this.model.getCurrentMeeting() != null) ? 'unavailable' : 'available');
    },
    serializeData: function () {
        var desc, state;

        var currentMeeting = this.model.getCurrentMeeting();
        if (currentMeeting != null) {
            //in currently meeting
            desc = 'Occupied';
            state = 'until ' + currentMeeting.EndTime.format('h:mm a'); //retrieve from current meeting item
        } else {
            desc = 'Available';
            var nextMeeting = this.model.getNextMeeting();
            if (nextMeeting != null) {
                state = 'until ' + nextMeeting.StartTime.format('h:mm a');//retrieve start time of next meet
            } else {
                state = 'for the rest of the day';
            }
        }

        return {
            IsAvailableDescription: desc,
            StateUntilTime: state,
            RoomName: Convenus.roomName //inject this into the model
        };
    }

});