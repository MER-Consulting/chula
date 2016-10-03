# Application

The application layer implements the API of the system. It typically exposes
 application services and API types. It never exposes domain types.

 (Application services and API types rarely benefit from separate interface
  types. Clients of application services are vertical infrastructure adapters
  and are as such better targets for integrations tests than unit tests.)

## Services

 Application services should closely follow use case scenarios.
 
 Application services use the domain to implement its services.
 
 Application services are responsible for handling some non-domain duties
  which need to encapsulate domain logic, for example:
 * Transactions
 * Authorization
 * Logging
 * Monitoring

### Use cases

Register customer

Update customer

Register vehicle

Update vehicle

Make reservation

Update reservation

Cancel reservation
