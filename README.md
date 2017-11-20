# Stargate File Upload Service
### A VueJS interface and .NET Core API for uploading files to storage

Currently supported File Storage Backend's

* Azure File Storage
* Azure Blob Storage

![Stargate Screenshot](https://i.imgur.com/tLfHqxk.png)

### Installation

1. Clone the repo
2. Build
  * **API**
     * Build the solution using Visual Studio 2017
  * **Web**
     * `npm install`
     * `npm run dev`

### Configuration

**Web**

Set the `API_HOST` and `API_PORT` values in the `config/dev.env.js`

**API**

If you're using Azure storage you will need to create an environment variable with your Connection String.

`stargate:azureStorageAccount` : _Connection string from Azure_

```
TIP: You can find your connection string from the "Access keys" menu option for your storage account
```

**_Troubleshooting_**

You may need to restart the npm http server or Visual Studio to pick up environment variable/configuration changes.

### Roadmap

* Unit Tests
* Hosted Example
* Built in URL Shortner
* Plugin Support
