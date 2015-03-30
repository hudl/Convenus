define(['marionette', 'radio', 'js/Spark/device-item-view.js'],
	function(Marionette, Radio, DeviceItemView) {
		var deviceCollectionView = Marionette.CollectionView.extend({
		    itemView: DeviceItemView
		});

		return deviceCollectionView;
	}
);