#include <Wire.h>
#include <BH1750.h>

BH1750 lightSensor1(0x23);
BH1750 lightSensor2(0x23);

float JoyX = 512;  // 初始值设为 512
float alpha = 0.5; // 平滑因子 (0.0 ~ 1.0，越小越平滑)

void setup() {
    Serial.begin(9600);
    Wire.begin();
    Wire1.begin();

    lightSensor1.begin(BH1750::CONTINUOUS_HIGH_RES_MODE, 0x23, &Wire);
    lightSensor2.begin(BH1750::CONTINUOUS_HIGH_RES_MODE, 0x23, &Wire1);

    Serial.println(F("BH1750 Test begin"));
}

void loop() {
    float lux1 = lightSensor1.readLightLevel();
    float lux2 = lightSensor2.readLightLevel();

    float targetJoyX = (lux1 - lux2) * 10 + 512;
    JoyX = alpha * targetJoyX + (1 - alpha) * JoyX;

    Joystick.X(JoyX);

    Serial.print("Sensor 1: ");
    Serial.print(lux1);
    Serial.print(", Sensor 2: ");
    Serial.print(lux2);
    Serial.print(", JoyX: ");
    Serial.println(JoyX);

    delay(50);
}