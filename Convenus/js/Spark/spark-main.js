define(['marionette', 'radio', 'js/Spark/login-view.js', 'js/Spark/device-collection-view.js', 'js/Spark/device-collection.js', 'js/Spark/device-control-view.js'],
	function(Marionette, Radio, LoginView, DeviceCollectionView, DeviceCollection, DeviceControlView) {
		var sparkApp = new Marionette.Application();

		sparkApp.addRegions({
	        login: '#login_container',
	        devices: '#device_container',
	        deviceControl: '#device_control_container'
		});

		sparkApp.addInitializer(function() {
			var loginView = new LoginView();
			this.login.show(loginView);
		});

		sparkApp.addInitializer(function() {
			var loginChannel = Radio.channel('login');
			loginChannel.on('login:complete', function() {
			    getDevices(function (devices) {
				    sparkApp.devices.show(new DeviceCollectionView({
				        collection: new DeviceCollection(devices)
				    }));
				});
			});

			function getDevices(onComplete) {
				spark.listDevices().then(onComplete);
			}
		});

		sparkApp.addInitializer(function() {
			var deviceChannel = Radio.channel('devise');
			deviceChannel.on('device:clicked', function(id) {
			    sparkApp.deviceControl.show(new DeviceControlView({
			        id: id
			    }));
			});
		})

		sparkApp.devices.on('show', function() {
			$('#devices').show();
		});

		sparkApp.start();
	}
);