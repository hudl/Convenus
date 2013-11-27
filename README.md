Convenus
========


A basic web application to show meeting room status. Runs self-hosted Nancy server. Windows/Mono compatible.

On Windows you may need to open the hosting port as admin before running. 
Example: 
`netsh http add urlacl url=http://+:8080/app user=domain\user`


You may run Convenus with a config file (-f option) or specify all the options as command line parameters (see [StartupOptions.cs](Convenus/StartupOptions.cs)).

CSS styling depends on Sass with assistance from [Bourbon](http://bourbon.io) and [Neat](http://neat.bourbon.io). Once [Sass is installed](http://sass-lang.com/install), run `sass --watch main.scss:main.css` to compile any changes.

Example configuration file (convenus.config):
```json
{
	UserName:"first.last@company-email.com",
	Password: "mySecurePassword",
	CompanyName: "MyCompany",
	Port: 1234,
	RequireAuth: true
}
```
