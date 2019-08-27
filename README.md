# IdentityRepo

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/65920d430cb440cb9d2ea0d752cf5d9f)](https://app.codacy.com/app/sadri-fertani/IdentityRepo?utm_source=github.com&utm_medium=referral&utm_content=sadri-fertani/IdentityRepo&utm_campaign=Badge_Grade_Settings)

Identity Server on IIS

## Grafana - Cloud
[Monitoring website](https://fertani.grafana.net)

## InfluxDb - Cloud
Management database website : [aiven.io](https://console.aiven.io/project/cedric-6b9c/services)

### Connexion :
```
influx -host '...' -port '18151' -username '...' -password '...' -database '...' -format json -ssl -pretty
```

### Install Cli :
```
brew install influxdb
```

### Documentation :
[Official documentation](https://docs.influxdb.com/influxdb/v1.7/tools/shell)

## App metrics
[Documentation](https://www.app-metrics.io)

### Test API Links :
* http://localhost:5001/metrics
* http://localhost:5001/metrics-text
* http://localhost:5001/env
* http://localhost:5001/health

## Cache
```
dotnet sql-cache create "Server=localhost;Database=DistCacheDB;Trusted_Connection=True;" dbo CacheTable
```