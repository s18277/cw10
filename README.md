# cw10
## Paweł Rutkowski s18277
W ramach tego ćwiczenia rozbudowałem projekt z [ćwiczenia 7](https://github.com/s18277/cw7).

W ćwiczeniu 10 dodałem nową implementację interfejsu `IDbStudentService` -
`Cw10.Services.DatabaseServices.EntityFrameworkCoreDbStudentService`. Implementacja ta wykorzystuje
`Entity Framework Core` zamiast bezpośrednich odwołań do bazy danych.

W ten sposób wszystkie istniejące końcówki są zrealizowane za pomocą `EFC`:
* Pobieranie listy wszystkich studentów
* Pobieranie pojedyńczego studenta
* Końcówka EnrollStudent
* Końcówka PromoteStudents

Jednocześnie wszystkie pozostałe odwołania do bazy danych są również wykonywane przez `EFC`.

Do kontrollera `Cw10.Controllers.StudentsController` dodałem dwie nowe końcówki - `UpdateStudent`
i `DeleteStudent`. Pozwalają one na aktualizację danych danego studenta i usunięcie danego studenta.

Obie są zrealizowane przy pomocy `EFC` w klasie
`Cw10.Services.DatabaseServices.EntityFrameworkCoreDbStudentService`.

Wykorzystane modele można zobaczyć w podkatalogu `Cw10/Models/StudentsDatabase/`. Znajduje się tam
również kontekst bazy danych `Cw10.Models.StudentsDatabase.StudentsDbContext`.
