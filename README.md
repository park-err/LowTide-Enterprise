# BrightLightEnt-API

## :gear: Configuration
Credentials need to be added via the [Google Cloud Console](https://console.cloud.google.com/apis/credentials?project=data-migration-2026-496913). We can use this to fill in our ```credentials.json```.


## :running: Running Locally

1. Clone the repository to a local directory. 
2. Copy and paste your GoogleAuth folder into the project folder[^1].
3. Run ```python3 google_auth.py``` to get your Google Id token
4. Open docker and run the application  

[^1]: Folder contains ```google_auth.py``` script and your ```credentials.json``` file. Remember to replace the client id and client secret with your info. Request access to the Shared Drive with this and download it.