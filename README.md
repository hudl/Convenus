Convenus
========

A basic web application to show meeting room status. Runs self-hosted Nancy server. Windows/Mono compatible.

On Windows you may need to open the hosting port as admin before running. 
Example: 
`netsh http add urlacl url=http://+:8080/app user=EVERYONE`


You may run Convenus with a config file (-f option) or specify all the options as command line parameters (see [StartupOptions.cs](Convenus/StartupOptions.cs)).

CSS styling depends on Sass with assistance from [Bourbon](http://bourbon.io), [Neat](http://neat.bourbon.io), [Skeleton](http://getskeleton.com/), and [Animate](http://daneden.github.io/animate.css/). Once [Sass is installed](http://sass-lang.com/install), run `sass --watch scss:css` in `/css` to compile any changes.

Example configuration file (convenus.config):
```json
{
	UserName:"first.last@company-email.com",
	Password: "mySecurePassword",
	CompanyName: "MyCompany",
	Port: 1234,
	RequireAuth: true,
	EnableSpark: false
}
```

Spark Core NeoPixel Display
========

Convenus integrates with spark cores which control [NeoPixel](https://www.adafruit.com/products/1138) light strips that can be placed around a window or door each meeting room. Each spark core is automatically updated with the status of each room when [a routine job](https://github.com/hudl/Convenus/blob/master/Sparkcore/job.ps1) is ran on the server.

When EnableSpark is set to true in the config or on the command line, then a new route `/spark` is enabled. This allows a user to manage multiple spark cores that have the Convenus firmware falshed on them (Firmware can be found [here](https://github.com/hudl/Convenus/tree/master/Sparkcore/Firmware)).