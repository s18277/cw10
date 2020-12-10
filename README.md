# cw7
## Paweł Rutkowski s18277

W ramach ćwiczenia 7 został dodany nowy kontroler - `Cw7.Controllers.LoginController`.

Zajmuje się on logowaniem nowych użytkowników i przydzielaniem im nowych tokenów `JWT`.
Kontroler ten umożliwia również tworzenie nowych tokenów na podstawie wygenerowanych wcześniej `refreshToken`.

Logowanie użytkowników jest dokonywane na podstawie przekazanego numeru indeksu i hasła użytkownika.

Hasła są przechowywane w bazie danych w postaci zahashowanej i soli.
Hasła nigdzie nie są przechowywane w formie tekstowej.

Weryfikacja przekazanego hasła jest dokonywana na podstawie porównania hashy haseł.

Do bazy danych przechowującej informacje o studentach dodałem dwie nowe tabele - `Role` i `RoleStudent`
realizującą relację wiele-do-wielu pomiędzy tabelami `Role` i `Student`.
Pozwalają one na ustalanie ról przydzielonych do użytkowników.
Na podstawie tych ról (konkretniej roli `employee`) tworzone są odpowiednie `Claims` w `JWT` i ograniczenie dostępu
do końcówek `Enroll` i `PromoteStudents`.

---

W ramach tego ćwiczenia usprawniłem nieco strukturę samej bazy danych.

Usunąłem niepotrzebne ograniczenia na kolumny o typach `nvarchar` i zmodyfikowałem kolumny typu `ID` kluczy głównych
tak, aby wykorzystywały właściwość `IDENTITY`.
Dzięki temu nie jest potrzebne określanie następnej wartości klucza głównego podczas dodawania wpisów do tabel.