# Clean Architecture Docker Angular Template

This template provides a starting point for creating a Clean Architecture solution with Docker and Angular. Follow the instructions below to use the template.

## Prerequisites

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for Angular)
- [Docker](https://www.docker.com/get-started)

## Getting Started

1. **Install the template:**

   ```bash
   dotnet new install <path-to-template>
   ```

2. **Create a new project using the template:**

   ```bash
   dotnet new cleanarch --projectName MyTestApp --namespacePrefix NitinSoft.
   ```

3. **Navigate to the project directory:**

   ```bash
   cd MyTestApp
   ```

4. **Build and run the project:**

   ```bash
   docker-compose up
   ```

## Template Parameters

- `--projectName`: The name of the project. Default is `MyApp`.
- `--namespacePrefix`: Optional namespace prefix (e.g., `MyOrg.`). Default is an empty string.

## Directory Structure

The template creates the following directory structure:

```
MyTestApp/
├── backend/
│   ├── src/
│   ├── tests/
│   └── ...
├── frontend/
│   ├── src/
│   ├── e2e/
│   └── ...
├── docker-compose.yml
└── README.md
```

## Customizing the Template

You can customize the template by modifying the `template.json` file located in the `template-src` directory. Refer to the [Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates) for more information on creating custom templates.

## Contributing

If you would like to contribute to this template, please follow the steps below:

1. **Create a new branch:**

   ```bash
   git checkout -b template-generator
   ```

2. **Add the folder structure:**

   - Add the `template-src/` folder with:
     - `CreateTemplate.ps1`
     - `template.json`
     - `README.template.md`
     - `.github/workflows/dotnet-template-publish.yml` (if you’re automating)

3. **Test locally:**

   ```bash
   cd template-src
   ./CreateTemplate.ps1
   ```

4. **Install and try the template:**

   ```bash
   dotnet new install ../src/ProjectTemplate
   dotnet new cleanarch --projectName MyTestApp --namespacePrefix NitinSoft.
   ```

5. **Check generated output:**

   Validate if:
   - Project files are renamed
   - Namespaces are updated
   - It builds and runs

6. **Commit and push:**

   ```bash
   git add .
   git commit -m "Add dynamic template generation"
   git push origin template-generator
   ```

Once you’re happy, you can either:
- Merge into main, or
- Keep it as a separate maintained branch for template packaging

## License

This project is licensed under the MIT License. See the [LICENSE](../LICENSE) file for details.
