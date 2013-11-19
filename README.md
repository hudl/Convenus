Convenus
========


A basic web application to show meeting room status. Runs self-hosted Nancy server. Windows/Mono compatible.

On Windows you may need to open the hosting port as admin before running. 
Example: netsh http add urlacl url=http://+:8080/app user=domain\user


You may run Convenus with a config file (-f option) or specify all the options as command line parameters (see StartupOptions class).

Example configuration file (convenus.config):

{
	UserName:"first.last@company-email.com",
	Password: "mySecurePassword",
	CompanyName: "MyCompany",
	Port: 1234,
	RequireAuth: true
}
