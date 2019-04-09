# Capstone
Created by Jason Detweiler
Email :jrd1326@gmail.com
Each Path includes a README File including wire connections, functionality and references to used material.

# User Interface
This project was created for a Computer Engineering Technology Capstone class at Conestoga College.
In its current state this UI communicates through a serial port to send and recieve data.

# Src - IOT REVB
This project was created for a Computer Engineering Technology Capstone class at Conestoga College.
This project Connects to the stm32l475 board. The board itself has wireless communication but is not yet used in this program
Currently this board outputs a PWM to control a motor.
The whole project is too big to upload to a single file so included are the main files included under the Src file in the project Directory.

Change Log
  -Revision B
    -Motor uses the L298 H-Bridge to control the speed and direction of the motor.
    -Added Full Motor Control with working motor.
    -Added and ADC to control the speed of the motor.
    -Added code to control the speed and direction of the motor.
    -Created functions to simulate real conditions on the road.
    -mode=1 will select the motor speed being controlled by the ADC
    -mode=0 will select the demo for real rod conditions.

# Src 
This project was created for a Computer Engineering Technology Capstone class at Conestoga College using - Stm32F411E board
This project currently Connects to a PC through a virtual com port and to the IOT board in Src-IOT through USart communication
Currently this project gets values from the PC and Displays them on 2,5 digit 7-segs and a single 7-seg.
The whole project is too big to upload to a single file so included are the main files included under the Src file in the project Directory.

Change Log
  -Revision B
    -Added Functionality to connect to F1 2018 and Read input from both the game and the Stm32F411E board
