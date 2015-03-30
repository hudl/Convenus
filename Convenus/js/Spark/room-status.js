define(function() {
	var RoomStatus = {
		Default: 0,
	    Taken: 1,
	    Available: 2,
	    EndOfMeeting: 3,
	    Party: 4
	}

    if (Object.freeze) {
        Object.freeze(RoomStatus);
    } else {
        console.log('Warning: Freezing objects not supported');
    }

    return RoomStatus;
});