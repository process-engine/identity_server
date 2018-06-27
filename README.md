# Identity Server

Der Identity Server ist die zentrale Autorität zur Verwaltung und Überprüfung
von Berechtigungen.

## Was sind die Ziele dieses Projekts?

Dieses Projekt stellt einen vorkonfigurierten IdentityServer bereit, der zu
Testzwecken genutzt werden kann.

## Wie kann ich das Projekt aufsetzen?

### Voraussetzungen

* .NET Core SDK 2.0 oder höher

### Setup/Installation

Zur Inbetriebnahme des Projekts müssen folgende Befehle ausgeführt werden:

1. dotnet restore               // stelle alle abhängigen Pakete wieder her
2. dotnet run                   // führe die Applikation aus

Sobald die Applikation gestartet ist, kann die Startseite unter
`http://localhost:5000` gefunden werden.

## Wie kann ich das Projekt benutzen?

### Nutzung des IdentityServer via HTTP

Zum Ausprobieren des IdentityServers beinhaltet dieses Projekt eine
Postman-Collection.

Diese enthält vorgefertigte HTTP-Requests, die zur Kommunikation mit dem
IdentityServer verwendet werden können.

#### Anmelden und Token beziehen

Dieser Request meldet sich mit folgenden Daten am IdentityServer an:
* client_id
* client_secret
* scope (optional)

Bei erfolgreicher Anmeldung erhält man den Statuscode 200 zurück und eine
Antwort, die den Token enthält:

```
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdmNTI3YmM1YjUyZTlmMDM5OGIzZTRkYzE4NmI2ZWE2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzAwODk4NjIsImV4cCI6MTUzMDA5MzQ2MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJzb21ldGhpbmciXSwiY2xpZW50X2lkIjoicHJvY2Vzcy1lbmdpbmUiLCJwaG9uZV9udW1iZXIiOiI2NjY2NjYtMjM0MjMtMzQyMyIsInNjb3BlIjpbInNvbWV0aGluZyJdfQ.0pYpiW7wzmt5hCEaOGFop5KXlIAiZkhpU3aa5d6hEl18zyU9WF2pXoKYnHN_C_OiFz1t9SZtw3W4N1Qz0LHqRjBYsumaTpBs5ByqEQL1X0JHgv-3lYQCpXMqxUoBTA9ET3VtAPMhQm4b-qRjxQRMLMsQkxL2xCx1MSFtOs-UUmYAGxjMIuw4-71fcn8V2ppqSzsCT23l-87saGtfTzN0GUez9281omYR8qKhKKqaJe4NKUiBGLt4wiQ6jutRZ75pQcn5DHrp3DICIE93eL3V0f0eLKq18CPK-_hcJo26wOJYoHu8t8yCR8eXBz8D-EDW_xMJVRKdb5RagwwxRmqtcw",
    "expires_in": 3600,
    "token_type": "Bearer"
}
```

Der Token wird automatisch in eine Environment-Variable in Postman eingetragen.
Weitere Requests, die eine Anmeldung benötigen, verwenden diesen Token dadurch
automatisch.

#### Claim validieren

Die Route `http://localhost:5000/claims/ensure/phone_number` wird für die
Validierung des Claims `phone_number` aufgerufen.

Der Token, der im `Authorization`-Header mitgegeben wird, wird vom
IdentityServer verwendet, um zu prüfen, ob die damit verbundene Identität diesen
Claim besitzt.

### Konfiguration von Benutzern, Rollen, Claims, etc...

Unter http://localhost:5000/admin/en-GB befindet sich die Admin-Oberfläche.

Sämtliche Konfigurationen, die der IdentityServer ermöglicht, können hierüber
vorgenommen werden.

Der IdentityServer verwendet eine lokale Datenbank. Werden Änderungen an der
Datenbank vorgenommen (bspw. durch Anlegen neuer Benutzer), so werden diese
direkt in der Datenbank gespeichert.

Wenn diese Änderungen mit anderen Personen geteilt werden sollen, kann die
Änderung an der lokalen Datenbank in dieses Repository eingecheckt werden.

### Vorkonfigurierte Daten

TODO

# Wen kann ich auf das Projekt ansprechen?

[Christoph Gnip](mailto:christoph.gnip@5minds.de)
[Robin Lenz](mailto:robin.lenz@5minds.de)
[Sebastian Meier](mailto:sebastian.meier@5minds.de)
[Christian Werner](mailto:christian.werner@5minds.de)