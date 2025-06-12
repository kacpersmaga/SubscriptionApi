# Subscription Api

## Skład grupy

- Kacper Smaga
- Roch Burmer
- Mateusz Caputa

## Krótki opis funkcji programu

Aplikacja webowa (REST API) do zarządzania subskrypcjami klientów. Główne funkcje:

- Dodawanie, edytowanie i usuwanie subskrypcji
- Pobieranie szczegółowych danych o subskrypcjach i klientach
- Powiązywanie subskrypcji z klientami
- Import danych klientów i subskrypcji z plików CSV

## Import danych z plików `.csv`

W katalogu `Infrastructure/Services/Csv` znajdują się dwie klasy:

- `CustomerCsvLoader` – odpowiada za ładowanie klientów
- `SubscriptionCsvLoader` – odpowiada za ładowanie subskrypcji

### Przykładowy format pliku `customers.csv`

```
Id,FirstName,LastName,Email,CreatedDate
1,Jan,Kowalski,jan.kowalski@email.com,2023-01-15
2,Anna,Nowak,anna.nowak@email.com,2023-02-20
```

### Przykładowy format pliku `subscriptions.csv`

```
CustomerId,CreatedDate,CanceledDate,SubscriptionCost,SubscriptionInterval,WasSubscriptionPaid
1,2023-02-01,,100,Month,Yes
2,2023-03-10,2023-06-10,200,Year,No
```

## Uruchomienie aplikacji

1. **Zbuduj projekt:**

   ```
   dotnet build
   ```

2. **Zastosuj migracje do bazy danych (jeśli wymagane):**

   ```
   dotnet ef database update
   ```

3. **Uruchom aplikację:**

   ```
   dotnet run --project WebAPI
   ```

4. **Dostęp do dokumentacji API (Swagger):**

   Otwórz w przeglądarce:

   ```
   https://localhost:5001/swagger
   ```

   lub

   ```
   http://localhost:5000/swagger
   ```

   W Swaggerze można testować wszystkie dostępne endpointy oraz wprowadzać dane wejściowe.

---

Projekt spełnia wymagania zarządzania subskrypcjami w oparciu o relacyjną bazę danych i umożliwia łatwy import danych wejściowych z plików CSV.
