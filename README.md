# FSM-Daemon

The FSM-Daemon (FileSystemManagement-Daemon) is used to make sure your files follow your **custom or a predefined** naming convention.

> [!warning]
> 🚧 This project is currently an **alpha version**. If you encounter any bugs or have some ideas and tips for improvement open an issue or a pull request. I would be happy to upgrade it and make it even better.

## Introduction

### How does it work?

The Daemon runs in the background and monitors all of your files specified. If you create, move or rename any of them the daemon will check the name and rename it if needed.

For example you only want your files to be snake_case and you create a file called `New File.txt` it will be automatically renamed to `new_file.txt`.
So, this daemon makes sure all your files follow a specific naming convention.

## Installation

You need to compile it by yourself as i am too lazy to do so.

Simply install the `dotnet` CLI tool and run the following command:

```shell
dotnet publish src/FileSystemMonitoring -c Release --self-contained /p:PublishTrimmed=true /p:PublishSingleFile=true -o ./out/release
```

Now your files are located in `out/release`. You can run the file now and watch the magic work.

## Configuration

To configure the daemon you need to edit the generated config file which is located in:

- **Linux:** `/home/$USER/.config/fmsmdaemon/config.yaml`

### Config Options

**Example config:**

```yaml
# Naming convention rules
namingRegex: "[^a-z0-9\\.]"
separator: "_"
lowercaseExtension: true
renameDelay: 100

# These paths will be monitored by the daemon but the excluded paths will be respected
includedPaths:
  - "~/Downloads"

# These paths will be ignored by the daemon
excludedPaths:
  - "bin"
  - "/tmp"
```

**Options:**

- `namingRegex`: The regex which will be applied to a sub-string of the filename. These specified characters will be replaced with the separator. Check out the [regex documentation](https://regular-expressions.info/) if you need help with the syntax.
- `separator`: The separator which will be used to split parts of the new filename. For example to replace special characters.
- `lowercaseExtension`: Whether to lowercase the extension to avoid something like `.PNG` which older programs might use.
- `renameDelay`: The delay to wait before renaming a file or folder.
- `includedPaths`: These paths will be monitored but excluded paths will still be ignored.
- `excludedPaths`: These paths will be excluded and therefore no file or folder within these folders will not be renamed.
