#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";
const char* hubUrl = "http://192.168.1.100/data";

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) delay(500);
}

void loop() {
  int pulse = random(60, 90);

  StaticJsonDocument<200> doc;
  doc["DeviceType"] = "pulse";
  doc["Value"] = pulse;

  String json;
  serializeJson(doc, json);

  HTTPClient http;
  http.begin(hubUrl);
  http.addHeader("Content-Type", "application/json");
  http.POST(json);
  http.end();

  Serial.println("Pulse sent");
  delay(5000);
}
