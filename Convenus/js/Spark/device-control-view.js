define(['marionette', 'radio', 'js/Spark/room-status.js'],
	function(Marionette, Radio, RoomStatus) {
		var deviceControlView = Marionette.ItemView.extend({
			template: '#device_control_template',
			templateHelpers: function () {
				return {
					available: RoomStatus.Available,
					taken: RoomStatus.Taken,
					endOfMeeting: RoomStatus.EndOfMeeting,
					error: RoomStatus.Default,
					party: RoomStatus.Party
				}
			},
			events: {
				'click button': 'onControlClick'
			},
			onControlClick: function(e) {
				spark.getDevice(this.options.id).then(function(device) {
					// parse device functions... access api... WOO
				});
			}
		});

		return deviceControlView;
	}
);