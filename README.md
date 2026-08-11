# LowTide-Enterprise

## :gear: Configuration
Credentials need to be added via the Google Cloud Console. We can use this to fill in our environment variables in ```.env```.


## :running: Running Locally

1. Clone the repository to a local directory. 
2. Configure the environment variables.
3. Ensure that the startup projects are LowTideEnt.Web and LowTideEnt.API[^1]
4. Open docker and run the application. A browser window will open with the application running automatically.

## :dart: Overview
LowTide-Enterprise is an application designed to support, automate, and integrate complex business operations across the fictional Low Tide organization.

## :rocket: Features
 - User Authentication
   - Google SSO authentication.
   - Supports role-based permission sets.
 - Resource Library
   - Ability for employees to view and search for resources.
   - Create, update, and remove resources with the proper permissions.

[^1]: Ensure that Multiple Startup Projects is configured. This is assuming you are running the application with Visual Studio.