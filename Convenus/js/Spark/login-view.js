define(['marionette', 'radio'], 
	function(Marionette, Radio) {
		var loginView = Marionette.ItemView.extend({
			template: '#login_template',
			className: 'animated fadeInDown',
			events: {
				'submit': 'sparkLogin'
			},
			initialize: function() {
				this.loginChannel = Radio.channel('login');
				this.$el.on('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', this.triggerComplete.bind(this));
			},
			sparkLogin: function(e) {
				this.login(this.$el.find('#spark_username').val(), this.$el.find('#spark_password').val());
				return false;
			},
			login: function(username, password) {
				spark.login({
					username: username,
					password: password
				}).then(function(response) {
					this.hideLogin();
				}.bind(this), function(err) {
					console.log('API call completed on promise fail: ', err.message);
				});
			},
			hideLogin: function() {
				this.$el.removeClass('fadeInDown').addClass('fadeOutUp');
			},
			triggerComplete: function() {
				if (this.$el.hasClass('fadeOutUp')) {
					this.$el.hide();
					this.loginChannel.trigger('login:complete');
				}
			}
		});

		return loginView;
	}
);