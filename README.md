# ovr-hear-forms
This project repository holds the Unity prefab for the research questionnaire forms.

Setup:
For compatibility, your Unity project requires the installation of the official Meta SDK.

1. Install Meta XR All-In-One SDK (Required)
You must install the Meta XR All-In-One SDK through the Unity Asset Store to ensure all dependencies for the prefabs are met.

Follow these steps:
Access the official SDK page on the Unity Asset Store: Meta XR All-In-One SDK

Log in, click 'Add to My Assets,' and then open your Unity project.

In your Unity Editor, navigate to Window > Package Manager.

Select 'Packages: My Assets' from the dropdown, locate the Meta XR All-In-One SDK, and click Download, then Import.

2. Import Prefab
Once the SDK is imported and configured, you can clone this repository and drag the provided questionnaire prefab into your Unity scene hierarchy to begin using the form.

NOTE: The logic for participant ID logging has not been fully implemented. It is up to you to adjust it to your own project. Additionally, if you were to design your own
Input UI, you must manually insert its logic into the QuestionnaireController.cs file, which is a script that manipulates UI elements and interactions.
