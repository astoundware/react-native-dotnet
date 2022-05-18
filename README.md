# React Native + .NET
A framework for building native apps with React and .NET.

## Getting Started
To get started, follow the platform-specific guide for each platform you want to target.
* [macOS](https://github.com/astoundware/react-native-macos-dotnet/blob/main/docs/getting-started.md)

## Contributing
Most contributions are welcome, but those not meeting the project's goals or standards may be rejected.

This project makes us of [Git LFS](https://git-lfs.github.com/) for storing binary files.  Please ensure that this is installed before cloning.

To begin, create a branch from `main` for the issue you are working on.  Please use the following naming convention.
> \<feature|bugfix\>/\<issue-number\>-\<short-description\>

If an issue does not exist for the improvement you would like to make, please create one.  Once work is complete, create a pull request to have the branch merged into `main`.

### Requirements
* [Git LFS](https://git-lfs.github.com/)
* [.NET 6+ SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Building and Packaging
* Use `dotnet build` to build the solution.
* Run `make` or `make pack` in the various project directories to build and package those libraries.

## License
This .NET project is provided under the [MIT License](LICENSE). React and React Native are copyright Facebook.
