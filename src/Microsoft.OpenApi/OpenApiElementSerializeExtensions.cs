﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Properties;
using Microsoft.OpenApi.Writers;

namespace Microsoft.OpenApi
{
    /// <summary>
    /// Represents the Open Api serializer.
    /// </summary>
    public static class OpenApiElementSerializeExtensions
    {
        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as JSON (v3.0)
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="stream">The output stream.</param>
        public static void SerializeAsJson<T>(this T element, Stream stream)
            where T : OpenApiElement
        {
            element.SerializeAsJson(stream, OpenApiSpecVersion.OpenApi3_0);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as JSON.
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specVersion">The Open Api specification version</param>
        public static void SerializeAsJson<T>(this T element, Stream stream, OpenApiSpecVersion specVersion)
            where T : OpenApiElement
        {
            element.Serialize(stream, specVersion, OpenApiFormat.Json);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as YAML (V3.0)
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="stream">The output stream.</param>
        public static void SerializeAsYaml<T>(this T element, Stream stream)
            where T : OpenApiElement
        {
            element.SerializeAsYaml(stream, OpenApiSpecVersion.OpenApi3_0);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as YAML.
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="specVersion">The Open Api specification version.</param>
        public static void SerializeAsYaml<T>(this T element, Stream stream, OpenApiSpecVersion specVersion)
            where T : OpenApiElement
        {
            element.Serialize(stream, specVersion, OpenApiFormat.Yaml);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream.
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="stream">The given stream.</param>
        /// <param name="specVersion">The Open Api specification version.</param>
        /// <param name="format">The output format (JSON or YAML).</param>
        public static void Serialize<T>(this T element, Stream stream, OpenApiSpecVersion specVersion, OpenApiFormat format)
            where T : OpenApiElement
        {
            if (stream == null)
            {
                throw Error.ArgumentNull(nameof(stream));
            }

            IOpenApiWriter writer;
            switch (format)
            {
                case OpenApiFormat.Json:
                    writer = new OpenApiJsonWriter(new StreamWriter(stream));
                    break;
                case OpenApiFormat.Yaml:
                    writer = new OpenApiYamlWriter(new StreamWriter(stream));
                    break;
                default:
                    throw new OpenApiException(String.Format(SRResource.OpenApiFormatNotSupported, format));
            }

            element.Serialize(writer, specVersion);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as JSON (v3.0)
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="writer">The output writer.</param>
        public static void Serialize<T>(this T element, IOpenApiWriter writer)
            where T : OpenApiElement
        {
            element.Serialize(writer, OpenApiSpecVersion.OpenApi3_0);
        }

        /// <summary>
        /// Serialize the <see cref="IOpenApiElement"/> to the given stream as JSON.
        /// </summary>
        /// <typeparam name="T">the <see cref="IOpenApiElement"/></typeparam>
        /// <param name="element">The Open Api element.</param>
        /// <param name="writer">The output writer.</param>
        /// <param name="specVersion">The Open Api specification version</param>
        public static void Serialize<T>(this T element, IOpenApiWriter writer, OpenApiSpecVersion specVersion)
            where T : OpenApiElement
        {
            if (element == null)
            {
                throw Error.ArgumentNull(nameof(element));
            }

            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            switch (specVersion)
            {
                case OpenApiSpecVersion.OpenApi3_0:
                    element.WriteAsV3(writer);
                    break;

                case OpenApiSpecVersion.OpenApi2_0:
                    element.WriteAsV2(writer);
                    break;

                default:
                    throw new OpenApiException(String.Format(SRResource.OpenApiSpecVersionNotSupported, specVersion));
            }

            writer.Flush();
        }
    }
}