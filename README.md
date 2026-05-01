# FSM-Daemon

The FSM-Daemon (FileSystemManagement-Daemon) is used to make sure your files follow your _custom or a predefined_ naming convention.

## Introduction

### How does it work?

It works by running in the background and monitoring all your files. And if you create, move or rename any of them the daemon will check the name of it and rename it it it needs to be changed.

For example you only want your files to be snake_case and you create a file called `New File.txt` it will be automatically renamed to `new_file.txt`.
So, this daemon makes sure all your files follow a specific naming convention.

## Configuration

This daemon comes with a CLI control client which is able to control the daemon via an _IPC Pipe_ and it works on Windows and Linux. _Probably on Mac too but I have not tested it yet..._

### CLI Commands

- `fsdaemon`:
  - `--help`: Shows the help command.
  - `--stop`: Stops the daemon.
  - `--start`: Starts the daemon.
  - `--restart`: Restarts the daemon and reloads the config file.
  - `--pause`: Pauses monitoring and returns to standby. **NOTE: The files created, moved or renamed in that time will not be renamed after continuing the monitoring.**
  - `--continue`: Continues the monitoring.
  - `--add-folder FOLDER_PATH`: Adds the specified folder to monitor where _FOLDER_PATH_ needs to be replaced by the folder you want to add.
  - `--remove-folder FOLDER_PATH`: Removes a folder from monitoring where _FOLDER_PATH_ needs to be replaced by the folder you want to remove.
  - `--change-naming-convention NAME`: Changes the naming convention of the files where _NAME_ needs to be replaced by the name of the convention.
  - `--add-naming-convention NAME NAMING_CONVENTION`: Adds a custom naming convention where _NAME_ needs to be replaced with the name and _NAMING_CONVENTION_ needs to be replaced by the new convention.
  - `--update-naming-convention NAME NAMING_CONVENTION`: Updates an existing naming convention where _NAME_ needs to be replaced by the name and _NAMING_CONVENTION_ by the new convention.
  - `--remove-naming-convention NAME`: Removes a custom naming convention where _NAME_ needs to be replaced with the name of the naming convention.
