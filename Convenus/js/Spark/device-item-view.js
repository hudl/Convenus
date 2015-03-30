define(['marionette', 'radio'], 
	function(Marionette, Radio) {
		var deviceItemView = Marionette.ItemView.extend({
			template: '#device_template',
			events: {
				'click .button': 'onDeviceClick'
			},
			initialize: function () {
				this.deviseChanel = Radio.channel('devise');
			},
			onDeviceClick: function (e) {
				this.deviseChanel.trigger('device:clicked', this.model.get('id'));
			}
		});

		return deviceItemView;
	}
);