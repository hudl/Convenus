//uses FullCalendar http://arshaw.com/fullcalendar/

var DayView = Backbone.Marionette.ItemView.extend({
    _updateInterval: null,
    _previousDayOfMonth: null,
    template: function() {
        return '';
    },
    modelEvents: {
        'change': '_fieldsChanged'
    },
    _fieldsChanged: function () {

        //if the day has changed - redraw the calendar
        if (this._previousDayOfMonth != null && this._previousDayOfMonth != moment().date()) {
            this._initCalendar();
        }
        
        //update events
        this.$el.fullCalendar('removeEvents');
        this.$el.fullCalendar('addEventSource', this.model.get('Events'));
 
        //var self = this;
//        _.each(this.model.get('Events'), function(event) {
//            self.$el.fullCalendar('renderEvent', self._transformEvent(event), true);
//        });
        //this.$el.fullCalendar('rerenderEvents');
    },
    initialize: function () {
        var self = this;
        $(function () {
            self._initCalendar.call(self);
        });
    },
    _initCalendar: function() {
        this.$el.empty();
        this.$el.fullCalendar(
          {
              defaultView: 'agendaDay',
              header: {
                  left: '',
                  center: 'title',
                  right: ''
              },
              allDaySlot: false,
              editable: false,
              aspectRatio: 0.33,
              minTime: 7, //these should be variables
              maxTime: 19,
              eventDataTransform: this._transformEvent
          });
        //update the current time line every minute
        this._updateCurrentTime();

        if (this._updateInterval) {
            window.clearInterval(this._updateInterval);
            this._updateInterval = 0;
        }

        this._updateInterval = window.setInterval(_.bind(this._updateCurrentTime, this), 1000 * 60);
        this._previousDayOfMonth = moment().date();
    },
    _updateCurrentTime: function() {
        var parentDiv = $(".fc-agenda-slots:visible", this.$el).parent();
        var timeline = parentDiv.children(".timeline");
        if (timeline.length == 0) { //if timeline isn't there, add it
            timeline = $("<hr>").addClass("timeline");
            parentDiv.prepend(timeline);
        }
        var curTime = new Date();

        var curCalView = this.$el.fullCalendar("getView");
        if (curCalView.visStart < curTime && curCalView.visEnd > curTime) {
            timeline.show();
        } else {
            timeline.hide();
        }
        var startSeconds = curCalView.getMinMinute() * 60;
        var endSeconds = curCalView.getMaxMinute() * 60;
        var curSeconds = (curTime.getHours() * 60 * 60) + (curTime.getMinutes() * 60) + curTime.getSeconds();
        var percentOfDay = (curSeconds - startSeconds) / (endSeconds - startSeconds);
        var topLoc = Math.floor(parentDiv.height() * percentOfDay);

        timeline.css("top", topLoc + "px");

        var dayCol = $(".fc-today:visible");
        var left = dayCol.position().left + 1;
        var width = dayCol.width();
        timeline.css({
            left: left + "px",
            width: width + "px"
        });
        
    },
    _transformEvent: function(eventData) {
        return {
            id: _.uniqueId(),
            title: eventData.Subject,
            start: eventData.StartTime.toDate(),
            end: eventData.EndTime.toDate(),
            allDay: false,
            className:'cal-event'
        };
    }

});