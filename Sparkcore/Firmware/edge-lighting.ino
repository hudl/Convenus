/*-------------------------------------------------------------------------

                     Spark Core NeoPixel Control

From the spark command line compile and flash with the command:
	spark flash edge_lighting edge-lighting.ino neopixel.cpp neopixel.h

To test an update status function use the spark command:
	spark call device_id updateRoom "0"

Room Status Mapping
	Unknown/Error: 		0
	Taken: 				1
	Available:			2
	End of Meeting:		3
	Party:				4

-------------------------------------------------------------------------*/

#include "neopixel.h"

#define PIXEL_PIN D2
#define PIXEL_COUNT 90
#define PIXEL_TYPE WS2812B
#define SWIPE_DELAY 5
#define RAINBOW_DELAY 5
#define PARTY_DELAY 500
#define LOADING_DELAY 50
#define SECONDS_BETWEEN_COLOR_SWITCH 5

Adafruit_NeoPixel strip = Adafruit_NeoPixel(PIXEL_COUNT, PIXEL_PIN, PIXEL_TYPE);

uint8_t counter = 0;
uint8_t isParty = 0;
uint8_t partyType = 0;

int updateRoom(String status) {
	isParty = 0;
	if (status == "0") {
		colorRed();
	} else if (status == "1") {
		colorBlue();
	} else if (status == "2") {
		colorGreen();
	} else if (status == "3") {
		colorYellow();
	} else if (status == "4") {
		isParty = 1;
	} else {
		colorRed();
		return -1;
	}
	return 0;
}

void setup() {
	strip.begin();
	strip.show();
	colorRed();
	Spark.function("led", updateRoom);
}

void loop() {
	counter++;
	if (isParty == 1) {
	    if (partyType == 0) {
		    party(PARTY_DELAY);
		    counter = counter % ((SECONDS_BETWEEN_COLOR_SWITCH * 2000) / PARTY_DELAY);
	    } else if (partyType == 1) {
		    rainbow(RAINBOW_DELAY);
		    counter = counter % ((SECONDS_BETWEEN_COLOR_SWITCH * 1000) / (RAINBOW_DELAY * 255));
	    } else if (partyType == 2) {
	        loading(LOADING_DELAY);
	        counter = 0;
	    }
	}
	
	if (counter == 0) {
	    partyType = (partyType+1) % 3;
	}
}

void colorRed() {
	colorWipe(strip.Color(255,0,0), SWIPE_DELAY);
}

void colorGreen() {
	colorWipe(strip.Color(0,255,0), SWIPE_DELAY);
}

void colorBlue() {
	colorWipe(strip.Color(0,0,255), SWIPE_DELAY);
}

void colorYellow() {
	colorWipe(strip.Color(255,255,0), SWIPE_DELAY);
}

void colorWipe(uint32_t c, uint8_t wait) {
    for(uint16_t i=0; i<strip.numPixels(); i++) {
        strip.setPixelColor(i, c);
    	strip.show();
    	delay(wait);
    }
}

void party(uint8_t wait) {
	uint16_t i;
	for (i = 0; i < strip.numPixels(); i++) {
		uint8_t r = rand() % 256;
		uint8_t g = rand() % 256;
		uint8_t b = rand() % 256;
		strip.setPixelColor(i, strip.Color(r,g,b));
	}
	strip.show();
	delay(wait);
}

void rainbow(uint8_t wait) {
	uint16_t i, j;

	for(j=0; j<256; j++) {
		for(i=0; i<strip.numPixels(); i++) {
			strip.setPixelColor(i, Wheel((i+j) & 255));
		}
		strip.show();
		delay(wait);
	}
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
	if(WheelPos < 85) {
		return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
	} else if(WheelPos < 170) {
		WheelPos -= 85;
		return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
	} else {
		WheelPos -= 170;
		return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
	}
}

void loading(uint8_t wait) {
    uint16_t fastLine=0, slowLine=0;
    uint16_t i, j;
    uint8_t delayVal = 2, delayCount = 0;
    int fastWidth = 3, slowWidth = 5;
    for(i=0; i<strip.numPixels()*delayVal; i++) {
        for(j=0; j<strip.numPixels(); j++) {
            uint8_t r=0,g=0,b=0;
            if(abs(j-fastLine) < fastWidth || (abs(j-(fastLine+strip.numPixels())) < fastWidth) || (abs(j-(fastLine-strip.numPixels())) < fastWidth)){
                r = 255;
            }
            int slowLine2 = (strip.numPixels() - slowLine - 1);
            if(abs(j-slowLine2) < slowWidth || (abs(j-(slowLine2+strip.numPixels())) < slowWidth) || (abs(j-(slowLine2-strip.numPixels())) < slowWidth)){
                g = 255;
            }
            if(r == 0 && g == 0){
                b = 255;
            }
            strip.setPixelColor(j, strip.Color(r,g,b));
    	}
    	fastLine = (fastLine+1) % strip.numPixels();
        delayCount = (delayCount+1) % delayVal;
        slowLine = delayCount == 0 ? (slowLine+1) % strip.numPixels() : slowLine;
    	strip.show();
	    delay(wait);
    }
}