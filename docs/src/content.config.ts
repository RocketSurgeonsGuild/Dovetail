import { defineCollection } from "astro:content";
import { docsSchema } from "@astrojs/starlight/schema";
import { readdirSync, statSync } from "fs";
import { changelogsLoader } from "starlight-changelogs/loader";
import { starlightTagsExtension } from "starlight-tags/schema";
import { dotnetXmlApiLoader } from "./loaders/dotnet-xml-api";

const sourceFiles = readdirSync("../src")
    .filter((d) => statSync(`../src/${d}`).isDirectory())
    .filter((z) => !z.includes(".Analyzers"))
    .map((d) => {
        return {
            projectDir: `../src/${d}`,
            assemblyName: d,
        };
    });

export const collections = {
    docs: defineCollection({
        // Custom loader: hand-written Markdown (via Starlight's docsLoader) plus API reference
        // pages parsed from each assembly's compiled XML documentation (all target frameworks).
        loader: dotnetXmlApiLoader({
            assemblies: sourceFiles,
            includeNamespaces: ["Dovetail", "Microsoft.Extensions"],
            basePath: "api",
        }),
        schema: docsSchema({ extend: starlightTagsExtension }),
    }),
    changelogs: defineCollection({
        loader: changelogsLoader([
            {
                provider: "github",
                base: "changelog",
                owner: "RocketSurgeonsGuild",
                repo: "Dovetail",
                token: import.meta.env.GH_API_TOKEN,
            },
        ]),
    }),
};
