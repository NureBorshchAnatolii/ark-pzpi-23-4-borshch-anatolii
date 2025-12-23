#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <MPU6050.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";
const char* hubUrl = "http://192.168.1.100/data";

MPU6050 mpu;

void setup() {
  Serial.begin(115200);
  Wire.begin(14, 12);

  mpu.initialize();

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) delay(500);
}

void loop() {
  int16_t ax, ay, az;
  int16_t gx, gy, gz;
  mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);

  long activity = abs(ax) + abs(ay) + abs(az);

  StaticJsonDocument<200> doc;
  doc["DeviceType"] = "activity";
  doc["Value"] = activity;

  String json;
  serializeJson(doc, json);

  HTTPClient http;
  http.begin(hubUrl);
  http.addHeader("Content-Type", "application/json");
  http.POST(json);
  http.end();

  Serial.println("Activity sent");
  delay(5000);
}
