define(['backbone', 'js/Spark/device-model.js'],
	function(Backbone, DeviceModel) {
	    var DeviceCollection = Backbone.Collection.extend({
	        model: DeviceModel
	    });
		return DeviceCollection;
	}
);