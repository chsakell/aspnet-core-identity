# ASP.NET Core Identity Series

## The most complete guide for ASP.NET Core Identity 

![ASP.NET Core Identity Series](https://chsakell.files.wordpress.com/2018/04/aspnet-core-identity-13.png)

[![License](https://img.shields.io/github/license/chsakell/aspnet-core-identity.svg)](https://github.com/chsakell/aspnet-core-identity/blob/master/LICENSE) [![Build status](https://ci.appveyor.com/api/projects/status/44f1gsf3quf0fbw2/branch/master?svg=true
)](https://ci.appveyor.com/project/chsakell/aspnet-core-identity)


## Part 1 - [Getting Started](http://chsakell.com/2018/04/28/asp-net-core-identity-series-getting-started)

* Introduction to ASP.NET Core Identity library
* Describe ASP.NET Core Identity basic archirecture
* Explain the role and relationship between `Stores` and `Managers` and how they function under the hood
* Explain what `Claims`, `ClaimsIdentity` and `ClaimsPrincipal` entities are and how they are related
* Step by step guide on how to install and start using the core packages
* Associated repository branch: [getting-started](https://github.com/chsakell/aspnet-core-identity/tree/getting-started)

## Part 2 - [Integrate Entity Framework](https://wp.me/p3mRWu-1i4)

* Introduce `Microsoft.Extensions.Identity.Stores` and `UserStoreBase` store implementations
* Plug and configure Entity Framework Core with ASP.NET Core Identity and minimum configuration
* Explain Entity Framework different store implementations such as `UserOnlyStore` or `UserStore`
* Step by step guide for applying migrations and creating Identity's SQL Schema
* Discuss whether you should use ASP.NET Core Identity with Entity Framework
* Associated repository branch: [entity-framework-integration](https://github.com/chsakell/aspnet-core-identity/tree/entity-framework-integration)

## Part 3 - [Deep Dive in authorization](https://wp.me/p3mRWu-1ik)

* Explain `Claims-based` authorization by example
* Explain `Role-based` authorization by example
* Step by step guide for creating custom `Authorization Policy Provider`
* Explain how authorization works under the hood
* Explain `Imperative authorization` by example
* Associated repository branch: [authorization](https://github.com/chsakell/aspnet-core-identity/tree/authorization)

## Part 4 - [OAuth 2.0, OpenID Connect & IdentityServer](https://wp.me/p3mRWu-1Ag)

* Explain how `OAuth 2.0` works *(terminology, grant types, tokens)*
* Explain how `OpenID Connect` works *(terminology, tokens, flows)*
* Learn how to use `IdentityServer` for integrating  `OAuth 2.0` and `OpenID Connect`
* Associated repository branch: [identity-server](https://github.com/chsakell/aspnet-core-identity/tree/identity-server)

## Part 5 - [External provider authentication & registration strategy](https://wp.me/p3mRWu-1Kq)

* Step by step guides for enabling external provider authentication
  *  [Google authentication](https://wp.me/p3mRWu-1Kq#google)
  *  [Facebook authentication](https://wp.me/p3mRWu-1Kq#facebook)
  *  [Twitter authentication](https://wp.me/p3mRWu-1Kq#twitter)
  *  [Microsoft authentication](https://wp.me/p3mRWu-1Kq#microsoft)
  *  [GitHub authentication](https://wp.me/p3mRWu-1Kq#github)
  *  [LinkedIn authentication](https://wp.me/p3mRWu-1Kq#linkedin)
  *  [DropBox authentication](https://wp.me/p3mRWu-1Kq#dropbox)
* Implement an external provider [registration strategy](https://wp.me/p3mRWu-1Kq#registration-strategy)
* Associated repository branch: [external-authentication](https://github.com/chsakell/aspnet-core-identity/tree/external-authentication)

## Part 6 - [Two Factor Authentication]()


> To be continued..

## Installation instructions

The project is built with ASP.NET Core with Angular on the client side. 
1. **Basic project setup**:
    * `cd ./AspNetCoreIdentity` where the package.json file exist
    * `npm install`
    * `dotnet restore`
    * `dotnet build`
    * `dotnet run`
2. **Create the *AspNetCoreIdentityDb* database** *(skip if you want to run with In memory DB)*
    * `cd ./AspNetCoreIdentity` where the AspNetCoreIdentity.csproj exist
    * `Add-Migration initial_migration` or `dotnet ef migrations add initial_migration`
    * `Update-Database` or `dotnet ef database update`
3. **Create the *IdentityServerDb* database** *(skip if you want to run with In memory DB)*
    * Follow the [instructions](https://github.com/chsakell/aspnet-core-identity/blob/master/IdentityServer/Data/Migrations/instructions.md)

> In case you don't want to use a real SQL Server Database when running the `AspNetCoreIdentity` project, simply set **InMemoryProvider: true** in the *appsettings.json*. This option will use in memory database

> In case you don't want to use a real SQL Server Database when running the `IdentityServer` project simply set **UseInMemoryStores: true** in the relative *appsettings.json* This option will use in memory database

<h3 style="font-weight:normal;">Follow chsakell's Blog</h3>
<table id="gradient-style" style="box-shadow:3px -2px 10px #1F394C;font-size:12px;margin:15px;width:290px;text-align:left;border-collapse:collapse;" summary="">
<thead>
<tr>
<th style="width:130px;font-size:13px;font-weight:bold;padding:8px;background:#1F1F1F repeat-x;border-top:2px solid #d3ddff;border-bottom:1px solid #fff;color:#E0E0E0;" align="center" scope="col">Facebook</th>
<th style="font-size:13px;font-weight:bold;padding:8px;background:#1F1F1F repeat-x;border-top:2px solid #d3ddff;border-bottom:1px solid #fff;color:#E0E0E0;" align="center" scope="col">Twitter</th>
</tr>
</thead>
<tfoot>
<tr>
<td colspan="4" style="text-align:center;">Microsoft Web Application Development</td>
</tr>
</tfoot>
<tbody>
<tr>
<td style="padding:8px;border-bottom:1px solid #fff;color:#FFA500;border-top:1px solid #fff;background:#1F394C repeat-x;">
<a href="https://www.facebook.com/chsakells.blog" target="_blank"><img src="https://chsakell.files.wordpress.com/2015/08/facebook.png?w=120&amp;h=120&amp;crop=1" alt="facebook" width="120" height="120" class="alignnone size-opti-archive wp-image-3578"></a>
</td>
<td style="padding:8px;border-bottom:1px solid #fff;color:#FFA500;border-top:1px solid #fff;background:#1F394C repeat-x;">
<a href="https://twitter.com/chsakellsBlog" target="_blank"><img src="https://chsakell.files.wordpress.com/2015/08/twitter-small.png?w=120&amp;h=120&amp;crop=1" alt="twitter-small" width="120" height="120" class="alignnone size-opti-archive wp-image-3583"></a>
</td>
</tr>
</tbody>
</table>
<h3>License</h3>
Code released under the <a href="https://github.com/chsakell/aspnet-core-identity/blob/master/LICENSE" target="_blank"> MIT license</a>.
