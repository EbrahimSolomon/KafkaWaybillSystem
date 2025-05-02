📨 Kafka Waybill Import System

🧾 Overview
This assessment includes 3 applications:
🚀 Application 1 - FileProcessor: Monitors the Incoming folder, reads .txt files line by line, and pushes content to Kafka.
📥 Application 2 - MessageConsumer: Listens to Kafka and inserts Waybills and Parcels into a PostgreSQL database.
🔍 Application 3 - WaybillSearchApp: A Blazor Server app to search and manage waybill and parcel data.

🔧 Prerequisites
🟦 .NET 8 SDK
🐘 PostgreSQL
🐳 Apache Kafka & Zookeeper
💾 EF Core Tools

⚙️ Setup
Start Kafka and Zookeeper:
zookeeper-server-start.bat config\zookeeper.properties
kafka-server-start.bat config\server.properties
Create Kafka Topic:
kafka-topics.bat --create --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic file-events
Configure appsettings.json in each app to match your environment (Kafka, PostgreSQL connection strings, etc).
Run EF Migrations:
cd WaybillSearchApp
dotnet ef database update

📁 Folder Structure
📂 FileProcessor
📂 MessageConsumer
📂 WaybillSearchApp

▶️ How to Run
FileProcessor
cd FileProcessor
dotnet run
Place a test file in:
FileProcessor/bin/Debug/net8.0/Incoming/test_data.txt
Example contents:
WB123|Express|Cape Town|8001|Johannesburg|2000|P001|30|20|15|2.5
WB999|Standard|Durban|4001|Pretoria|0001|P002|15|25|35|2.5
WB999|Standard|Durban|4001|Pretoria|0001|P003|12|22|32|1.7

MessageConsumer
cd MessageConsumer
dotnet run
WaybillSearchApp
cd WaybillSearchApp
dotnet run
Access at: http://localhost:<port>/

📝 Notes
New files dropped into the Incoming folder will be processed continuously.
Waybills are upserted and each parcel is uniquely tied to one waybill.
EF Core handles upserts to ensure idempotent writes.

👨‍💻 Author
This application is made with ❤️ by Ebrahim Solomon

https://github.com/EbrahimSolomon/KafkaWaybillSystem