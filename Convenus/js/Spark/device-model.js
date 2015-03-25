define(['backbone'],
	function() {
		var DeviceModel = Backbone.Model.extend({
			defaults: {
				_spark : {
					accessToken: ''
				},
				connected: false,
				id: '',
				name: 'Unknown Core'
			}
		});
		return DeviceModel;
	}
);