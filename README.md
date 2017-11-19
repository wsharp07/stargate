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

If you're using Azure storage you will need to create an environment variable with your Connection String.

**Name:** stargate:azureStorageAccount

**Value:** _Connection string from Azure_

### Roadmap

* Unit Tests
* Hosted Example
* Built in URL Shortner
* Plugin Support
