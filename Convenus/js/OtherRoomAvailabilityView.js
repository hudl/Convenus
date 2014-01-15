var OtherRoomAvailabilityView = Backbone.Marionette.ItemView.extend({
    template: '#other-availability-template',
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {
        this.render();
    },
    serializeData: function () {
        var available = !this.model.getCurrentMeeting();

        return {
            IsAvailable: available,
            AvailableRooms: this.model.get('AvailableRooms')
        };
    }

});