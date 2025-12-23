#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <DHT.h>

#define DHTPIN 4
#define DHTTYPE DHT22

const char* ssid = "Wokwi-GUEST";
const char* password = "";
const char* hubUrl = "http://192.168.1.100/data";

DHT dht(DHTPIN, DHTTYPE);

void setup() {
  Serial.begin(115200);
  dht.begin();

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) delay(500);
}

void loop() {
  float temp = dht.readTemperature();
  if (isnan(temp)) return;

  StaticJsonDocument<200> doc;
  doc["DeviceType"] = "temperature";
  doc["Value"] = temp;

  String json;
  serializeJson(doc, json);

  HTTPClient http;
  http.begin(hubUrl);
  http.addHeader("Content-Type", "application/json");
  http.POST(json);
  http.end();

  Serial.println("Temperature sent");
  delay(5000);
}
