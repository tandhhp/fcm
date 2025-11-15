Language : English | [Tiếng Việt](./README.md)

<h1 align="center">Waffle</h1>

- Front-end: https://github.com/f7deat/cms

## 📦 Development

GIT:

```bash
$ git clone https://github.com/f7deat/waffle.git
```

FE:

```bash
$ cd ClientApp
$ npm i
$ npm run build
```

BE:

```bash
$ dotnet build
$ dotnet ef migrations add InitialCreate
$ dotnet ef database update
```

Learn more about Migration: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/

Open your browser and visit http://localhost:8000

## 🚀 Deployment

web.config: allow cors
```
</system.webServer>
    ...
    <security>
        <requestFiltering>
          <verbs>
            <remove verb="OPTIONS" />
            <add verb="OPTIONS" allowed="true" />
          </verbs>
        </requestFiltering>
      </security>
</system.webServer>
```

## 🔨 Usage

`waffle` is available supported in live server: https://cms.defzone.net

## 📝 Docs

Read document:

| # | Name | URL                                 |
|---|------|-------------------------------------|
| 1 | API  | https://waffleverse.gitbook.io/api/ |

email: noreply@nuras.com.vn
password: 3M@ail0f764h37hc
server: mail.nuras.com.vn