# TP 8INF956 - Communication par messages

## Les projets

La solution est composé de 4 différents projets :
- **LogEmitter** : émet des logs aléatoires toutes les 2 sec. La partie "RabbitMQ" est abstraire par la classe `RabbitLogger` qui implémente l'interface `ILogger`.
```csharp
public interface ILogger
    public void Init();
    public void Log(string message, LogSeverity severity);
    public void Close();
```
- **FileDump** : sauvegarde les logs de sévérité `warning`, `error` et `critical` dans un fichier `application.log`. Utilise la classe `RabbitMQReceiver` du projet `LogReceiver` pour initier la connexion et recevoir les logs.
- **LogAnalysis** : Affiche les statistiques des logs emis. Utilise la classe `RabbitMQReceiver` du projet `LogReceiver` pour initier la connexion et recevoir les logs.
- **LogReceiver** : Abstrait la partie RabbitMQ des receveurs avec la classe `RabbitMQReceiver` qui implémente la l'interface `ILogReceiver`
```csharp
public interface ILogReceiver
    void Init();
    void BindRoute(LogSeverity severity);
    void Listen(Action<LogSeverity, string> onReceived);
    void Close();
```

## Utilisation

L'ordre de lancement des projets n'est pas important.

```
# Lancement de l'emetteur aléatoire de log
cd LogEmitter
dotnet run

# Lancement de l'analyseur de logs
cd LogAnalysis
dotnet run

# Lancement de la sauvegarde de l'historique logs de sévérité `warning`, `error` et `critical`
cd FileDump
dotnet run

# Exemple analyseur de logs:
info : 2 logs
warning : 4 logs
error : 1 logs
critical : 3 logs
Press any key to exit.

# Exemple d'historique des logs (fichier `application.log` situé dans le dossié d'exécution) :
[19-04-2020 22:48:23][ERROR]: Would you like to have some coffee?
[19-04-2020 22:48:25][ERROR]: He landed a big trout.
[19-04-2020 22:48:27][CRITICAL]: Your biggest enemy is yourself.
[19-04-2020 22:48:33][WARNING]: Shape up, or ship out!
```
