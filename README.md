# cw6
## Paweł Rutkowski s18277

W ramach ćwiczenia 6 dodałem do projektu dwa nowe middleware:
- inline w metodzie `Cw6.Startup.Configure` weryfikujące, czy w nadchodzących żądaniach znajduje
się nagłówek `Index` o wartości znajdującej się w bazie danych
- zewnętrzny `Cw6.Middlewares.LoggingMiddleware` logujący wszystkie nadchodzące żądania do pliku
`requestsLog.txt`

Dodatkowo dodałem prosty middleware obsługujący wszystkie wyjątki powstałe podczas przetwarzania
żądań `Cw6.Middlewares.ExceptionMiddleware`. Zwróci on odpowiedź w formacie JSON z informacją o
nastałym wyjątku.

Dodatkowo dodałem prostą dokumentację generowaną automatycznie za pomocą
[Swagger/OpenAPI](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).
Żądania odczytania dokumentacji nie wymagają specjalnego nagłówka (wymienionego powyżej), nie
zostaną też zalogowane.

Dokumentację w formacie graficznym można otworzyć przez ścieżkę `/swagger/index.html`.
Dokuemntację w formacie JSON można pobrać ze ścieżki `/swagger/v1/swagger.json`.