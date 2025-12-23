#include <WiFi.h>
#include <WebServer.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <time.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";
const char* remoteServerUrl = "";

WebServer server(80);

long lastPulse = 0;
int lastTemperature = 0;
const long deviceId = 1;

String getCurrentDateTime() {
  time_t now = time(nullptr);
  struct tm* t = gmtime(&now);
  char buf[25];
  strftime(buf, sizeof(buf), "%Y-%m-%dT%H:%M:%SZ", t);
  return String(buf);
}

void sendToRemote() {
  StaticJsonDocument<200> doc;

  doc["ReadDateTime"] = getCurrentDateTime();
  doc["Pulse"] = lastPulse;
  doc["Temperature"] = lastTemperature;
  doc["DeviceId"] = deviceId;

  String json;
  serializeJson(doc, json);

  HTTPClient http;
  http.begin(remoteServerUrl);
  http.addHeader("Content-Type", "application/json");
  http.POST(json);
  http.end();

  Serial.println("Sent IoTReading:");
  Serial.println(json);
}

void handleSensorData() {
  StaticJsonDocument<100> incoming;
  deserializeJson(incoming, server.arg("plain"));

  String type = incoming["Type"];
  long value = incoming["Value"];

  if (type == "pulse") {
    lastPulse = value;
  }

  if (type == "temperature") {
    lastTemperature = value;
  }

  sendToRemote();
  server.send(200, "text/plain", "OK");
}

void setup() {
  Serial.begin(115200);

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) delay(500);

  configTime(0, 0, "pool.ntp.org");

  server.on("/data", HTTP_POST, handleSensorData);
  server.begin();

  Serial.print("HUB IP: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  server.handleClient();
}
