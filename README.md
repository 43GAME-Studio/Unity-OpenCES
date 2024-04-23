# Unity OpenCES
Unity Open Customizable Environment System

A mobile platform environment system that works in the Built-in Render pipeline that offers built-in 4 weather and ambient sound effects and day/night cycles and volume and much more.

# Library
DOTween (HOTween v2)<br>
assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676<br>

Post Processing<br>
com.unity.postprocessing<br>

# Unity Asset Store
Nature Sound FX<br>
assetstore.unity.com/packages/p/nature-sound-fx-180413<br>

[BFW]Simple Dynamic Clouds<br>
assetstore.unity.com/packages/p/bfw-simple-dynamic-clouds-85665<br>

# How to use
This paragraph will teach you how to use the environment system<br>
(This usage applies to version 1.1 (Beta) and above)
> ## Import
> 1.Import all the dependencies given into your project<br>
> 2.Import this unity package<br>

> ## Delta timer (Used for day/night cycles)
> 1.Add the 43GAME Studio/Timer/Delta Timer/Delta Tiemr Updater component to any GameObject (It is recommended to add components on the EventSystem)<br>

> ## Volume
> 1.Add a new layer in Edit -> Project Settings -> Tags & Layers, Suggested Name: "Post Processing"<br>
> 2.Set the Layer field of the Post Process Layer for the Prefab Camera to the layer you just added<br>
> 3.Set the layer for the Prefab camera to the layer you just added<br>
> 4.Set the layer of the EnvironmentSystem Prefab to the layer you just added (while changing the children)<br>

> ## Follow the object
> 1.Set the followObject field of the EnvironmentController to the transform of the target object<br>

> ## Shortcomings
> 1.A non-strict datetime algorithm is used<br>
> 2.Insecure encapsulation<br>
> 3.May contain performance issues<br>
