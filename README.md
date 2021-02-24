# State-Machine-Callback
Adds a start and end callback to a state in the animator. Can be used to track when an animation state has started and ended.

## How to install
This package can be installed through the Unity `Package Manager` with Unity version 2019.3 or greater.

Open up the package manager `Window/Package Manager` and click on `Add package from git URL...`.

![unity_package_manager_git_drop_down](Documentation~/images/unity_package_manager_git_drop_down.png)

Paste in this repository's url.

`https://github.com/Jason-Skillman/State-Machine-Callback.git`

![unity_package_manager_git_with_url](Documentation~/images/unity_package_manager_git_with_url.png)

Click `Add` and the package will be installed in your project.

---
**NOTE:** For Unity version 2019.2 or lower

If you are using Unity 2019.2 or lower than you will not be able to install the package with the above method. Here are a few other ways to install the package.
1. You can clone this git repository into your project's `Packages` folder.
1. Another alternative would be to download this package from GitHub as a zip file. Unzip and in the `Package Manager` click on `Add package from disk...` and select the package's root folder.

---

## How to setup
First click on the animation state in your animator. Click `Add Behavior` and search for the `StateMachineEvent` component. This component will catch events from `StateMachineBehaviour` and send it through the `IStateMachineCallback` interface.

### Using the callbacks

Now that the callback behavior is attached to the animation state its time to use its callbacks. Create an empty script and attach it to the game object with the animator. Make sure that the script implements `IStateMachineCallback`. This will give you all of the callbacks that the `StateMachineEvent` component fires.

### Using multible animation states
Multible animation state callbacks can be used, just attach the `StateMachineEvent` component to each one in the animator. In your driver script that implements `IStateMachineCallback` you can use the `stateInfo` and `layerIndex` parameters to separate out the animator callbacks.

In this example there are two states in the animator called `TurnOn` and `TurnOff` which both have the `StateMachineEvent` component. You can use `stateinfo.IsName` method to check which state is currently animating. This sample can also be installed by using the package manager.

```C#
public void OnAnimationStart(AnimatorStateInfo stateInfo, int layerIndex) {
	//Use stateInfo to get the correct animation state name
	if(stateInfo.IsName("TurnOn")) {
		//...
	} else if(stateInfo.IsName("TurnOff")) {
		//...
	}
}
```

## IStateMachineCallback API
|Method Name|Parameters|Description|
|---|---|---|
|`void` `OnAnimationStart()`|stateInfo, layerIndex|Called once when the animation state starts.|
|`void` `OnAnimationUpdate()`|stateInfo, layerIndex|Called every frame while animating.|
|`void` `OnAnimationEnd()`|stateInfo, layerIndex|Called once when the animation state ends.|
