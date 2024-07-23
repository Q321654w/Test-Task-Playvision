# Playvision test task


Repository contains solution of [test task for Playvision](https://playvision.notion.site/Unity-06482e0128b341c1a1c13db137357d21). 
The project structure is based on features. 
It means that each feature have its own folder where placed scripts, prefabs, materials etc. for objects that support that feature.


Solution allow to simulate physical throwing dices with selected numbers.


### Structure

```
Test-Task-Playvision
├── Assets
│   ├───Animations
│   │   └───TransformAnimations
│   │       └───Recorders
│   ├───Dices
│   │   ├───Prefabs
│   │   │   └───Models
│   │   └───Views
│   ├───DiceThrowers
│   │   └───Editor
│   ├───Extensions
│   ├───Materials
│   ├───Scenes
│   └───TextMesh Pro
├── Packages
└─── ProjectSettings
```


* [PhysicalTransformAnimationRecorder.cs](Assets/Animations/TransformAnimations/Recorders/PhysicalTransformAnimationRecorder.cs)

  * Record physical transformations of game objects over time.

* [TransformAnimation.cs](Assets/Animations/TransformAnimations/TransformAnimation.cs)
  
  * Defines an animation based on transformations (position, rotation) of game objects.

* [TransformAnimationKey.cs](Assets/Animations/TransformAnimations/TransformAnimationKey.cs)
  
  * Represents a key frame in a transform animation.

* [IAnimation.cs](Assets/Animations/IAnimation.cs)

  * Defines a generic interface for animation objects.

* [DiceThrower.cs](Assets/DiceThrowers/DiceThrower.cs)

  * Handles the logic for throwing dice in the game.

* [DiceThrowerEditor.cs](Assets/DiceThrowers/Editor/DiceThrowerEditor.cs)

  * Provides custom editor functionality for the DiceThrower script in the Unity Editor.

* [Dice.cs](Assets/Dices/Dice.cs)

  * Represents a single dice in the game.

* [DiceView.cs](Assets/Dices/Views/DiceView.cs)

  * Handles the visual representation of a dice.

* [Vector3Extensions.cs](Assets/Extensions/Vector3Extensions.cs)
  
  * Provide randomize extension for Vector3.

***

### Demo 

https://github.com/user-attachments/assets/b7327c5e-0e80-4532-90c2-78ddc6198208

***

### Techno demo

https://github.com/user-attachments/assets/6a78a684-0084-43de-b634-01d869bf08ab
