# Arduino Four-Servo Controller

Arduino Four-Servo Controller is a C# Windows Forms desktop application for calibrating, and controlling four servo motors through an Arduino-compatible board.

This application was created to assist with the initial setup of a robotic arm equipped with four servos. It offers individual control for the base, elbow, shoulder, and gripper servos, and aids in the determination of safe movement limits and servo offsets prior to additional robotic-system integration.

## Download

The portable Windows executable is available through the project’s GitHub Releases page:

[Download the latest Arduino Four-Servo Controller release](https://github.com/Mario-Youchia/Arduino-Four-Servo-Controller/releases/latest)

The current portable release is:

```text
Arduino Four-Servo Controller v1.0.1
```

Download `ServosTest.exe` from the release assets.

## Preview

![Main Arduino Four-Servo Controller interface](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-main-interface.png)

The main interface provides independent controls for the base, elbow, shoulder, and gripper servos, together with PWM-pin selection and Arduino connection settings.

![Connected four-servo control interface](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-connected-control.png)

After connecting, the application displays the current angle of each servo and enables direct adjustment through sliders and increment/decrement controls.

![Arduino and servo wiring schematic](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-schematic.png)

The included schematic documents the wiring between the Arduino board and the four servo motors.

![Soft-reset and hard-reset controls](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-reset-controls.png)

The reset menu provides a soft reset for restoring the interface state and a hard reset for clearing stored calibration limits.

![Application help window](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-help-window.png)

The built-in Help window explains the default configuration, servo calibration process, and hardware-safety precautions.

![Application information window](public/images/projects/arduino-four-servo-controller/arduino-four-servo-controller-about-window.png)

The About window says the application is a calibration tool for robotic arms with four servo motors.

## Main Features

* Independent control of the base, elbow, shoulder, and gripper servos
* Selectable and mutually exclusive PWM-pin assignments
* Arduino board, serial-port, and baud-rate configuration
* Automatic upload of embedded Arduino firmware
* Real-time servo-angle transmission through serial communication
* Minimum and maximum angle calibration for each servo
* Persistent calibration settings between sessions
* Default configuration based on the included schematic
* Soft-reset and hard-reset options
* Input validation and connection-loss handling
* Help, and About windows
* Portable Windows executable alongside the complete source project

## Technical Overview

The desktop application is written in C# using Windows Forms and targets .NET 6 for Windows.

The interface manages four servo channels:

| Channel | Role     |
| ------- | -------- |
| Servo 1 | Base     |
| Servo 2 | Elbow    |
| Servo 3 | Shoulder |
| Servo 4 | Gripper  |

Each servo is assigned a distinct PWM pin. The application prevents the same selected pin from being assigned to more than one servo.

The supported board profiles are:

* Arduino Leonardo
* Arduino Mega 1284
* Arduino Mega 2560
* Arduino Micro
* Arduino Nano R2
* Arduino Nano R3
* Arduino Uno R3

Fifteen selectable serial baud rates are supported, ranging from 300 to 2,000,000 baud. A corresponding compiled Arduino firmware image is embedded for every supported baud rate.

When the user connects to a board, the application:

1. Validates the board, serial port, baud rate, and PWM-pin selections.
2. Extracts the matching embedded firmware image to a temporary location.
3. Uploads the firmware to the selected Arduino board.
4. Opens the serial connection using the selected baud rate.
5. Sends the current angles of all four servo motors whenever a control value changes.

The default configuration applies the following setup:

| Setting        | Default value        |
| -------------- | -------------------- |
| Base servo     | Pin 3                |
| Elbow servo    | Pin 5                |
| Shoulder servo | Pin 6                |
| Gripper servo  | Pin 9                |
| Board          | Arduino Uno R3       |
| Serial port    | First available port |
| Baud rate      | 9600                 |

Servo limits are stored in `Settings.txt` when the application closes. The application also creates `History Log.txt` to record application activity. Both files are generated at runtime beside the executable.

The source project uses:

* `ArduinoUploader`
* `IntelHexFormatReader`
* `System.IO.Ports`
* `System.Management`

## How to Run

### Portable executable

The portable Windows executable is distributed through GitHub Releases:

[Download the latest release](https://github.com/Mario-Youchia/Arduino-Four-Servo-Controller/releases/latest)

Download this release asset:

```text
ServosTest.exe
```

To use it:

1. Download `ServosTest.exe` from the latest GitHub release.
2. Connect a supported Arduino board through USB.
3. Connect the four servos according to the included schematic.
4. Run `ServosTest.exe`.
5. Select **Default** to apply the documented configuration, or choose the settings manually.
6. Select **Connect** and allow the firmware-upload process to finish.
7. Adjust the servos gradually and define safe minimum and maximum limits.

The application will create these runtime files beside the executable:

```text
Settings.txt
History Log.txt
```

The release executable is a large single-file Windows build, it is intended to be self-contained, i.e. work without installation of any package.

### Source project

Requirements:

* Windows
* Visual Studio with .NET desktop development support
* .NET 6 SDK
* A supported Arduino board and its required USB serial driver

Open:

```text
source/ServosTest.sln
```

Build the solution, and run the Windows Forms project.

The embedded firmware files under:

```text
source/ServosTest/Test/
```

Do not remove/modify these files, they are required for the automatic firmware-upload feature.

## Limitations

* The application is designed for Windows.
* Successful hardware operation depends on correct and appropriate setup.
* Servo limits should be calibrated gradually to avoid motor damage.
* The application is only used for direct control and calibration, but it is not a complete robotic-arm motion-planning system.
