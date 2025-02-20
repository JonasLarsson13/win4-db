# Projekt Hanteringssystem - Backend API

Detta är ett backend API byggt med .NET Core och Entity Framework Core för att hantera projekt, anställda, kunder och tjänster. Systemet möjliggör CRUD-operationer och använder en SQL Server-databas för lagring.
### SQL Servern körs i Docker.

-----------------------------------------------------------------

## Funktionalitet
Systemet stöder följande operationer:

### Skapa entiteter
- Statusar
- Roller (Måste inkludera "Projektledare" för att kunna välja en projektledare till ett projekt)
- Tjänster
- Kunder
- Anställda (Måste ha rollen "Projektledare" för att kunna väljas som projektledare)

### Hantera projekt
- Skapa projekt (Måste ha en giltig projektledare)
- Redigera projekt (Alla fält utom projektnummer kan ändras)
- Ta bort projekt

-----------------------------------------------------------------

## Flöde för att skapa ett projekt
För att kunna skapa ett projekt behöver följande steg genomföras i denna ordning:

1️⃣ Skapa en status (Exempel: "Ej påbörjat", "Pågående", "Avslutat")
2️⃣ Skapa en roll (Exempel: "Projektledare", "Utvecklare")
3️⃣ Skapa en tjänst (Exempel: "Konsultarbete", "Design")
4️⃣ Skapa en kund (En kund måste ha kontaktuppgifter)
5️⃣ Skapa en anställd (Om anställd ska vara projektledare, sätt rollen till "Projektledare")
6️⃣ Skapa ett projekt (Alla fält fylls i och en projektledare väljs)