# Notes for the operational team of the application

## Urls

The frontend of the application runs on [macedonieje.maartenprojecten.nl](http://macedonieje.maartenprojecten.nl), the
backoffice runs on [admin.macedonieje.maartenprojecten.nl](http://admin.macedonieje.maartenprojecten.nl)

## Tools

We have a few tools running in our cluster that make debugging and seeing
backend results easier. Since application/infrastructure security
is not a priority nor something we are graded on, these services run
exposed to the internet on a subdomain, rather than behind a firewall.

### PgAdmin

To make testing database records easy we've decided to
add PgAdmin to both our development and the test environment. This
admin interface is running over at
[pgadmin.macedonieje.maartenprojecten.nl](http://pgadmin.macedonieje.maartenprojecten.nl)

The username is `admin@kantilever.nl` and the password is `d38y89yhkufh38y83f3h88kehfa`.

The connection host name of the server is `postgres-svc`, the user is `postgres` and
the password is `r4q8o78q78278lriLUO3249U8R3`.

### RabbitMQ Management interface

For the same reasons why we decided to add in PgAdmin, we've
also implemented the RabbitMQ Management interface in our current non-production
environments. This one can be found running over at
[rabbitmq.macedonieje.maartenprojecten.nl](http://rabbitmq.macedonieje.maartenprojecten.nl)

The username is `rabbitmq-admin` and the password is `nmcnjklhai7a3kjhkjlljeahf`.

### EFK Stack

In the alpha version of the application there was an EFK stack running in the cluster to
keep track of logs and errors. Due to budget and resource constraints we were asked to deactivate and remove this
component.

### PostgresQL

Due to cluster limitations and the fact that we have to share
resources we are unable to properly implement an autonomous microservice instance with
a dedicated database, instead we are forced to use one database server
with multiple databases. Since the existing system utilizes a PostgresQL database, we've also chosen to use this
particular database software, no need to start up another database if we can just use the existing one.

## Security

This application does not use an encrypted message broker or a HTTPS connection. The reason for this is that
the scope of the minor does not include infrastructure security according to the product owner and adding in
TLS would not have granted us a higher score for this case. A 'real' system would obviously use HTTPS and a TLS
encrypted message broker. For the same reasons we've also not encrypted any secrets in our manifests files,
we only encoded them so that they could be used in the k8s environment.

We've also scattered a few comments in our code indicating security hotspots that could be improved in case
this system were to go live.

## Credentials

To log into the backoffice's frontend application you need to use either one of these usernames:
- kees
- esther
- dennis
- timo

The passwords of these users are `Pass123$`.

## Service configuration

### Frontend
- BROKER_CONNECTION_STRING: Connection string to the RabbitMQ Broker
- BROKER_EXCHANGE_NAME: Name of the main exchange
- BROKER_REPLAY_EXCHANGE_NAME: Name of the replay exchange
- AUDITLOGGER_URL: Url to the auditlogger
- DB_CONNECTION_STRING: Connection string to the postgres database
- CATALOGUS_SERVICE_URL: Url to the catalogus service
- VOORRAAD_SERVICE_URL: Url to the voorraadservice
- AUTH_AUTHORITY: Url to the identity service for the backend
- ANGULAR_AUTHORITY: Url to the identity service for the frontend
- ANGULAR_CLIENTID: The id of the idetntiy client
- ANGULAR_RESPONSE_TYPE: Type of response from the identity
- ANGULAR_REDIRECT_URI: Callback after login
- ANGULAR_SCOPE: Scope for the token
- ANGULAR_POST_LOGOUT_REDIRECT_URI: Redirect url after logout

### Bestel
- BROKER_CONNECTION_STRING: Connection string to the RabbitMQ Broker
- BROKER_EXCHANGE_NAME: Name of the main exchange
- BROKER_REPLAY_EXCHANGE_NAME: Name of the replay exchange
- AUDITLOGGER_URL: Url to the auditlogger
- DB_CONNECTION_STRING: Connection string to the postgres database

### Backoffice
- BROKER_CONNECTION_STRING: Connection string to the RabbitMQ Broker
- BROKER_EXCHANGE_NAME: Name of the main exchange
- BROKER_REPLAY_EXCHANGE_NAME: Name of the replay exchange
- AUDITLOGGER_URL: Url to the auditlogger
- DB_CONNECTION_STRING: Connection string to the postgres database
- AUTH_CLIENT_SECRET: Client secret for identityservice
- AUTH_CLIENT_ID: Client id for identityservice
- AUTH_AUTHORITY: Url to identity service
- CATALOGUS_SERVICE_URL: Url to the catalogus service
- VOORRAAD_SERVICE_URL: Url to the voorraadservice

### klant
- BROKER_CONNECTION_STRING: Connection string to the RabbitMQ Broker
- BROKER_EXCHANGE_NAME: Name of the main exchange
- DB_CONNECTION_STRING: Connection string to the postgres database

### Identity
- BROKER_CONNECTION_STRING: Connection string to the RabbitMQ Broker
- BROKER_EXCHANGE_NAME: Name of the main exchange
- DB_CONNECTION_STRING: Connection string to the postgres database
- CONFIG_PATH_CLIENTS: Path inside the container of the clients' json file
- CONFIG_PATH_IDS: Path inside the container of the resources' json file
- CONFIG_PATH_APIS: Path inside the container of the apis' json file

In kubernetes, clients.json, apis.json and ids.json are mounted through configmaps inside the container.
These files are then loaded into memory at startup.
The schematic of these files can be found in the Data folder in service's directory.
