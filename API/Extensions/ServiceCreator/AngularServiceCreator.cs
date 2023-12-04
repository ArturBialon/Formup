﻿using Application.Controllers.Base;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using NSwag.Generation.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Application.Extensions.ServiceCreator
{
    public static class AngularServiceCreator
    {
        private const string relativeFilePath = @"..\client\src\app\_services\autogenerated\autogenerated.service.ts";
        private const string assemblyName = "Application";

        public async static Task ConfigureSwaggerAsync()
        {
            var baseDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativeFilePath));

            var swaggerSpecification = await CreateSwaggerSpecification();
            await CreateJson(swaggerSpecification, fullPath);
        }

        private static async Task<string> CreateSwaggerSpecification()
        {
            var settings = new WebApiOpenApiDocumentGeneratorSettings
            {
                DefaultUrlTemplate = "api/{controller}/{action}/{id?}"
            };
            var generator = new WebApiOpenApiDocumentGenerator(settings);
            var controllers = GetAllControllersInfoAssembly();
            var document = await generator.GenerateForControllersAsync(controllers);
            var swaggerSpecification = document.ToJson();

            return swaggerSpecification;
        }

        private static IEnumerable<Type> GetAllControllersInfoAssembly()
        {
            return Assembly.Load(assemblyName).GetTypes().Where(type => typeof(ApiControllerBase).IsAssignableFrom(type));
        }

        private static async Task CreateJson(string json, string path)
        {
            var document = await OpenApiDocument.FromJsonAsync(json);
            var generator = new TypeScriptClientGenerator(document, GetSettings());
            var code = generator.GenerateFile();
            File.WriteAllText(path, code);
            InterfaceConverter.ReadFile(path);

        }

        private static TypeScriptClientGeneratorSettings GetSettings()
        {
            var settings = new TypeScriptClientGeneratorSettings();

            settings.TypeScriptGeneratorSettings.TypeScriptVersion = 4.3M;
            settings.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
            settings.Template = TypeScriptTemplate.Angular;
            settings.RxJsVersion = 6.0M;
            settings.HttpClass = HttpClass.HttpClient;
            settings.UseSingletonProvider = true;
            settings.InjectionTokenType = InjectionTokenType.InjectionToken;
            settings.GenerateDtoTypes = true;
            settings.TypeScriptGeneratorSettings.ExportTypes = true;
            settings.TypeScriptGeneratorSettings.DateTimeType = TypeScriptDateTimeType.Date;
            settings.TypeScriptGeneratorSettings.MarkOptionalProperties = true;
            settings.TypeScriptGeneratorSettings.NullValue = TypeScriptNullValue.Null;
            settings.ClassName = "{controller}Service";
            settings.GenerateClientInterfaces = true;

            return settings;

        }

    }
}