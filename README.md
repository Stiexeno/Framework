# Framework

Framework is a lightweight and flexible framework for Unity, designed to simplify development and promote modularity through dependency injection.

## Features

- Dependency injection for easy and decoupled component communication.
- Simple and intuitive API for defining and injecting dependencies.
- Improved testability and maintainability of Unity projects.
- Supports Unity 2019.4 or later.

## Installation

You can install MyFramework via the Unity Package Manager by following these steps:

1. In Unity, open your project.
2. Open the Unity Package Manager from the "Window" menu.
3. Click on the "+" button and select "Add package from git URL".
4. Enter the following URL: `https://github.com/Stiexeno/Framework.git`
5. Press Enter or click on the "Add" button.
6. Unity will fetch and import the package into your project.

Alternatively, you can manually download the package from the [Releases](https://github.com/Stiexeno/Framework/releases) page and import it into your Unity project.

## Usage

### Injecting Dependencies

To define a dependency, simply annotate your methods with the `[Inject]` attribute:

```csharp
using Framework.Core;
using UnityEngine;

public class NPCSystem : MonoBehaviour
{
    private IAssets assets;
    
    [Inject]
    private void Construct(IAssets assets)
    {
        this.assets = assets;
    }

    // Rest of your code...
}
```

### Binding Dependencies

To bind a dependency, you should create `IBindingInstaller` as plain class:

```csharp
using Framework;
using Framework.Core;

public class GameplayInstaller : IBindingInstaller
{
    public void InstallBindings(DiContainer diContainer)
    {
        diContainer.Bind<SceneManager>();
    }
}
```

## Binding usages

### Bind for later usage
To bind a service and instantiate it only when it is requested, use the `Bind<T>` method:

```csharp
diContainer.Bind<DataManager>();
```

The above code binds the `DataManager` class to its corresponding interface or base class. The DI container will create an instance of `DataManager` when it is requested for the first time.

### Binding and Immediate Instantiation

If you want to instantiate a service immediately when the DI container is set up, you can use the `Instantiate<T>` method:

```csharp
diContainer.Bind<LocalClock>().Instantiate();
```

The above code binds the `LocalClock` class to its corresponding interface or base class and immediately creates an instance of `LocalClock`. This is useful for services that need to be initialized at the start of the application.

### Finding References from Scene

In some cases, you may want to find a reference to a service that already exists in the scene. To do this, you can use the `FindInScene<T>` method:

```csharp
diContainer.Bind<UIManager>().FindInScene<UIManager>();
```

The above code binds the `UIManager` class to its corresponding interface or base class and finds a reference to an existing `UIManager` component in the scene.

## License

This framework is released under the MIT License. See the LICENSE file for more details.

