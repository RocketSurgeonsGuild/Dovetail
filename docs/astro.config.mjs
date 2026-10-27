import { unified } from "@astrojs/markdown-remark";
import starlight from "@astrojs/starlight";
import { defineConfig } from "astro/config";
import starlightAutoDrafts from "starlight-auto-drafts";
import starlightChangelogs from "starlight-changelogs";
import starlightGithubAlerts from "starlight-github-alerts";
import starlightHeadingBadges from "starlight-heading-badges";
import starlightImageZoom from "starlight-image-zoom";
import starlightLinksValidator from "starlight-links-validator";
import starlightLlmsTxt from "starlight-llms-txt";
import starlightPageActions from "starlight-page-actions";
import { starlightIconsPlugin } from "starlight-plugin-icons";
import starlightScrollToTop from "starlight-scroll-to-top";
import starlightSidebarTopics from "starlight-sidebar-topics";
import starlightTags from "starlight-tags";

// In GitHub Actions, the site is served under /Dovetail; locally we run at the root.
const base = process.env.GITHUB_ACTIONS ? "/Dovetail" : "";

export default defineConfig({
    site: "https://rocketsurgeonsguild.github.io",
    base,
    markdown: {
        processor: unified(),
    },
    integrations: [
        starlight({
            title: "Dovetail",
            description: "Dovetail-based configuration and composition for .NET applications.",
            logo: {
                light: "./src/assets/dovetail-wordmark-light.svg",
                dark: "./src/assets/dovetail-wordmark-dark.svg",
                alt: "Dovetail",
                replacesTitle: true,
            },
            social: [
                {
                    icon: "github",
                    label: "GitHub",
                    href: "https://github.com/RocketSurgeonsGuild/Dovetail",
                },
            ],
            customCss: ["./src/styles/theme.css", "./src/styles/api-reference-site.css", "./src/styles/api-reference.css"],
            plugins: [
                starlightAutoDrafts(),
                starlightGithubAlerts(),
                starlightSidebarTopics(
                    [
                        {
                            label: "Getting Started",
                            link: "/guides/",
                            icon: "open-book",
                            items: [{ autogenerate: { directory: "guides" } }],
                        },
                        {
                            label: "Concepts",
                            link: "/concepts/",
                            icon: "information",
                            items: [{ autogenerate: { directory: "concepts" } }],
                        },
                        {
                            label: "API Reference",
                            link: "/api/",
                            icon: "code-branch",
                            items: [{ label: "Search C# APIs", link: "/api/" }],
                        },
                        {
                            label: "Changelog",
                            link: "/changelog/",
                            icon: "list-format",
                        },
                    ],
                    {
                        // The API reference pages (src/pages/api/) build their own package/namespace/type sidebar.
                        exclude: ["/tags/**", "/changelog/**", "/api", "/api/**"],
                    },
                ),
                starlightLinksValidator({
                    errorOnRelativeLinks: false,
                    // The API reference pages are Astro routes (src/pages/api/) generated from the build's JSON, not
                    // content entries, so this plugin can't see them.
                    exclude: ["/api", "/api/**"],
                }),
                starlightHeadingBadges(),
                starlightImageZoom(),
                starlightScrollToTop(),
                starlightPageActions({
                    editLink: "https://github.com/RocketSurgeonsGuild/Dovetail/edit/main/docs/src/content/docs/",
                }),
                starlightIconsPlugin(),
                starlightTags(),
                starlightChangelogs(),
                starlightLlmsTxt(),
            ],
        }),
    ],
});
