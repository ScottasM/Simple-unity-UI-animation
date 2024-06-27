# Simple-unity-UI-animation

![](https://github.com/ScottasM/Simple-unity-UI-animation/assets/68515176/a67de043-f056-4fba-85a8-b4c03472bbac)


A rather simple UI animation script for unity, that allows animating UI using slide(movement) and scale using animation curves and a few other settings. On top of that - it also allows you to group multiple UI objects that can be animated using scale during the animation of the original. 

# Installation
Just donwload the UIAnimation.unitypackage and add it to your project.

# Usage
Add AnimateUI script to any UI object.
Get the reference of the AnimateUI script.
Call ```Show(bool immediately = false, Action OnOpen = null)``` to open and ```Hide(bool immediately = false, Action OnClose = null)``` to close.
Use OnOpen or OnClose for Actions on animation end. 


# Example
With the settings in the photo, i got a result like in the GIF

![](https://github.com/ScottasM/Simple-unity-UI-animation/assets/68515176/2f0c7452-ec2e-4243-aedb-9d0421da70fd)

