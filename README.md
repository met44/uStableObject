# uStableObject, when 1 / reacts
Unity3D ScriptableObject based architecture framework.

ScriptableObject based architecture simplify wiring elements and systems together while also bringing the benefits of decoupling. It can also ease sharing data accross scenes in a simple, clean, generic an reliable way. 

No more fiddly unappliable cross prefab references in scene objects, everything pings pongs through your defined assets. Never suffer from a broken reference again !


# Getting Started

## GameEvent
Creating a GameEvent
- Right click in project, goto Create > uStableObject > GameEvent
- Choose type and click to create a GameEvent asset

Creating a GameEvent listener as an asset (lives in project)
- Right click in project, goto Create > uStableObject > GameEvent > AssetListeners
- Choose matching type and click to create an asset listener
- Plug the GameEvent in the GameEvent field of the listener
- Setup the UnityEvent on the listener to trigger desired feature

Creating a GameEvent listener as a monobehaviour (lives in scene)
- Create a GameObject, add a GameEventListener of the matching type (searching "event listener" works well to list all available types)
- Plug the GameEvent in the GameEvent field of the listener
- Setup the UnityEvent on the listener to trigger desired feature

Firing a GameEvent
- From a script with a reference to the GameEvent asset, call myGameEvent.Raise();
- It may eventually expect a parameter depending on choosen type when creating the GameEvent, for example myTransformEvent.Raise(this.transform);


## Vars
Vars store data of the specified type, and also act as a GameEvent of that type. For exemple setting myBoolVar.Value = true; will also fire the GameEvent as if calling myBoolVar.Raise(true) on a bool event

Vars work with standard GameEvent listeners, but also have a specific type of listener called Watcher that also fires the value when the object is enabled.
