# Wallet

Descomprimir aquí los archivos:

- cwallet.sso
- ewallet.p12
- sqlnet.ora
- tnsnames.ora

## Actualizar datos

Verificar la configuración del archivo sqlnet.ora:

```text
WALLET_LOCATION = (SOURCE = (METHOD = file) (METHOD_DATA = (DIRECTORY="?/Wallet")))
SSL_SERVER_DN_MATCH=yes
```

## Agregar conexión a Oracle

Agregar la cadena de conexión en el archivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=<user>;Password=<password>;Data Source=<tns_connection>;Wallet_Location=Wallet"
  }
}
```