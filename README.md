# MyFramework

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

MyFramework is a lightweight and flexible framework for Unity, designed to simplify development and promote modularity through dependency injection.

## Features

- Dependency injection for easy and decoupled component communication.
- Simple and intuitive API for defining and injecting dependencies.
- Improved testability and maintainability of Unity projects.
- Supports Unity 20XX.X or later.

## Installation

You can install MyFramework via the Unity Package Manager by following these steps:

1. In Unity, open your project.
2. Open the Unity Package Manager from the "Window" menu.
3. Click on the "+" button and select "Add package from git URL".
4. Enter the following URL: `https://github.com/username/repo.git`
5. Press Enter or click on the "Add" button.
6. Unity will fetch and import the package into your project.

Alternatively, you can manually download the package from the [Releases](https://github.com/username/repo/releases) page and import it into your Unity project.

## Usage

### Defining Dependencies

To define a dependency, simply annotate your MonoBehaviour class with the `[Inject]` attribute:

```csharp
using MyFramework;
using UnityEngine;

public class MyComponent : MonoBehaviour
{
    [Inject]
    private MyDependency myDependency;

    // Rest of your code...
}
