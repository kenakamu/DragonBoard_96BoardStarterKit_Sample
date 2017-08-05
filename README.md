# Dragon Board 410c Windows 10 IoT Core with 96 Board Starter Kit sample
Dragon Board 410c Windows 10 IoT Core with 96 Board Starter Kit sample

This repository contains sample UWP application for DragonBoard 410c and sensors with Linker Mezzanine card. Unlike other samples, this application connects many sensors listed below to demonstrate how to utilize multiple sensors.

### DragonBoard 410c 
<img src="https://www.96boards.org/product/ce/dragonboard410c/images/DragonBoard-UpdatedImages-front.png" width="200">

Click [here](https://www.96boards.org/product/dragonboard410c) for more detail.

### Linker Mezzanine card starter kit for 96board
<img src="http://static.chip1stop.com/img/product/LINS/800px-Arrow3874.JPG" width="200">

Click [here](http://linksprite.com/wiki/index.php5?title=Linker_Mezzanine_card_starter_kit_for_96board) for more detail.

### DragonBoard 410c Pin and Mezzanine card diagram
<p>
<img src="https://az835927.vo.msecnd.net/sites/iot/Resources/images/PinMappings/DB_Pinout.png" width="300">
<img src="http://linksprite.com/wiki/images/c/c7/1-4.jpg" width="500">
</p>

From the diagram, you can see pin number for each Degital Interface
- D1 -> 36
- D2 -> 13
- D3 -> 115
- D4 -> 24

## How to install Windows 10 IoT Core for DragonBoard
Follow the instruction [here](https://developer.microsoft.com/en-us/windows/iot/getstarted)

## Device Setup
In this sample, plug sensors as below.

- Touch Sensor -> D1
- LED -> D2
- Button -> D3
- Tilt Sensor -> D4
- TPM36 -> ADC1
- Photoresistor -> ADC2

## Sensors 
#### Output
- LED: Red LED and two onboard Green LEDs

#### Input: Switch
- Touch sensor: When you touch the sensor, it lights up LEDs and get data from sensors.
- Button: When you push the button, it lights up LEDs and get data from sensors.
- Tilt Module: When you tilt the module one way, it lights up LEDs and get data from sensors.

#### Input: Sensor
- TPM36: Measure temperature
- Photoresistor: Measuare the brightness

## Lisence
MIT
