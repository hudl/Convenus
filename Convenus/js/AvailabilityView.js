var AvailabilityView = Backbone.Marionette.ItemView.extend({
    template: '#room-availability-template',
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {
        this.render();
    },
    onRender: function () {
        var currentMeeting = this.model.getCurrentMeeting();
        var nextMeeting = this.model.getNextMeeting();

        var newClass = '';
        if (currentMeeting != null) {
            newClass = 'unavailable';
        }
        else if (nextMeeting != null && moment().add('minutes', 5).isAfter(nextMeeting.StartTime)) {
            //if room will be blocked within 5 min - change style to indicate
            //if you don't want to support this feature just set make pending-occupied equal to available
            newClass = 'pending-occupied';
        } 
        else {
            newClass = 'available';
        }
        
        this.$el.removeClass('unavailable available pending-occupied').addClass(newClass);
    },
    serializeData: function () {
        var desc, state;

        var currentMeeting = this.model.getCurrentMeeting();
        var nextMeeting = this.model.getNextMeeting();
        
        if (currentMeeting != null) {
            //in currently meeting
            desc = 'Occupied';
            state = 'until ' + currentMeeting.EndTime.format('h:mm a'); //retrieve from current meeting item
        } else {
            desc = 'Available';
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