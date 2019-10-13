# Migration instructions for IdentityServer project

`cd path-to\IdentityServer`

## Migrations using terminal

### Create migrations
* `dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb`
* `dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb`
* `dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ApplicationDbContext -o Data/Migrations`

### Update database
* `dotnet ef database update --context ApplicationDbContext`
* `dotnet ef database update --context PersistedGrantDbContext`
* `dotnet ef database update --context ConfigurationDbContext`


## Migrations using  VISUAL STUDIO Package Manager Console

### Create migrations
* `Add-Migration InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -Project "IdentityServer" -StartUpProject "IdentityServer" -o Data/Migrations/IdentityServer/PersistedGrantDb`
* `Add-Migration InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -Project "IdentityServer" -StartUpProject "IdentityServer" -o Data/Migrations/IdentityServer/ConfigurationDb`
* `Add-Migration InitialIdentityServerConfigurationDbMigration -c ApplicationDbContext -Project "IdentityServer" -StartUpProject "IdentityServer" -o Data/Migrations`

### Update database
* `Update-Database -Context ApplicationDbContext -Project "IdentityServer" -StartupProject "IdentityServer"`
* `Update-Database -Context PersistedGrantDbContext -Project "IdentityServer" -StartupProject "IdentityServer"`
* `Update-Database -Context ConfigurationDbContext -Project "IdentityServer" -StartupProject "IdentityServer"`
