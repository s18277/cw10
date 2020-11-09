# cw4
## Paweł Rutkowski s18277
W tym zadaniu dodałem implementację interfejsu `Cw4.DAL.IDbService` korzystającą z bazy danych
MSSQL - `Cw4.DAL.MssqlDbService`.

Kontroler `Cw4.Controllers.StudentsController` umożliwia pobieranie rekordów z bazy danych
poprzez metodę `GET`, wstawianie nowych rekordów poprzez metodę `POST`, aktualizację rekordu
metodą `PUT` (wykorzystujac zarówno *URL-segment* i ciało wiadomości) oraz usuwanie rekordów
poprzez metodę `DELETE` (wykorzystując *URL segment*).

Metoda `GET` wraz z przkazanym numerem indeksu studenta (przez URL-Segment) zwróci informacje
o tym studencie wraz z danymi dotyczącymi semestru i kierunku studiów na który jest zapisany.
Dane te są połączeniem danych ze wszystkich trzech tabel.
