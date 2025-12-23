#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <Wire.h>
#include <DHT.h>
#include <MPU6050.h>
#include <time.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";

const char* controlUrl = "";
const char* dataUrl    = "";

#define DHTPIN 4
#define DHTTYPE DHT22
DHT dht(DHTPIN, DHTTYPE);

MPU6050 mpu;

const unsigned long SEND_INTERVAL = 5000;
unsigned long lastSendTime = 0;

long simulatePulse() {
  return random(60, 90);
}

long calculateActivityLevel() {
  int16_t ax, ay, az;
  int16_t gx, gy, gz;
  mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
  return abs(ax) + abs(ay) + abs(az);
}

void setupTime() {
  configTime(0, 0, "pool.ntp.org");
  while (time(nullptr) < 100000) {
    delay(500);
  }
}

String getCurrentDateTime() {
  time_t now = time(nullptr);
  struct tm* tm = gmtime(&now);
  char buf[25];
  strftime(buf, sizeof(buf), "%Y-%m-%dT%H:%M:%SZ", tm);
  return String(buf);
}

bool isSendingAllowed() {
  HTTPClient http;
  http.begin(controlUrl);

  int code = http.GET();
  if (code != 200) {
    http.end();
    return false;
  }

  StaticJsonDocument<64> doc;
  deserializeJson(doc, http.getString());
  http.end();

  return doc["isActive"] | false;
}

void setup() {
  Serial.begin(115200);
  Wire.begin(14, 12);

  dht.begin();
  mpu.initialize();

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
  }

  setupTime();
}

void loop() {
  if (millis() - lastSendTime < SEND_INTERVAL) return;
  lastSendTime = millis();

  if (!isSendingAllowed()) {
    Serial.println("Sending disabled by server");
    return;
  }

  int temperature = (int)dht.readTemperature();
  if (isnan(temperature)) temperature = 0;

  StaticJsonDocument<256> doc;
  doc["readDateTime"] = getCurrentDateTime();
  doc["activityLevel"] = calculateActivityLevel();
  doc["pulse"] = simulatePulse();
  doc["temperature"] = temperature;
  doc["deviceId"] = 1;

  String payload;
  serializeJson(doc, payload);

  HTTPClient http;
  http.begin(dataUrl);
  http.addHeader("Content-Type", "application/json");
  int code = http.POST(payload);
  http.end();

  Serial.println(payload);
  Serial.println("POST status: " + String(code));
}
