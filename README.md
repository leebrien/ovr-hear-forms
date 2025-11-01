# OVR-Hear-Forms

This project repository holds the **Unity prefab** for research questionnaire forms designed for VR environments.

---

# Setup

For compatibility, your Unity project requires the installation and configuration of the official **Meta SDK**.

## 1. Install Meta XR All-In-One SDK (Required)

You must install the Meta XR All-In-One SDK through the Unity Asset Store to ensure all dependencies for the prefabs are met.

Follow these steps:

1.  Access the official SDK page on the Unity Asset Store: [**Meta XR All-In-One SDK**](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269223)
2.  Log in, click **'Add to My Assets,'** and then open your Unity project.
3.  In your Unity Editor, navigate to **Window > Package Manager**.
4.  Select **'Packages: My Assets'** from the dropdown, locate the Meta XR All-In-One SDK, and click **Download**, then **Import**.

## 2. Import Prefab

Once the SDK is imported and configured, you can clone this repository and **drag the provided questionnaire prefab** into your Unity scene hierarchy to begin using the form.

---

# Note on Customization

* **Participant ID Logging:** The logic for participant ID logging has not been fully implemented. You will need to adjust this functionality to fit your own project's requirements.
* **Custom UI:** If you design your own Input UI, you must manually insert its logic into the `QuestionnaireController.cs` file, which is the core script that manipulates UI elements and interactions.

---

# Credits / Attribution

This project was developed by:

* Brodett, Ram David
* David, Peter Jan
* Lopez, Ghee Kaye
* Sandoval, Lee Brien
