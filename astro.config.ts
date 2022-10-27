import vercel from "@astrojs/vercel/static";
import { defineConfig } from "astro/config";
import partytown from "@astrojs/partytown";
import prefetch from "@astrojs/prefetch";
import robotsTxt from "astro-robots-txt";
import remarkGithub from "remark-github";
import sitemap from "@astrojs/sitemap";
import preact from "@astrojs/preact";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
    output: "static",
    adapter: vercel(),
    integrations: [
        preact({
            compat: true
        }),
        image({
            serviceEntryPoint: "@astrojs/image/sharp"
        }),
        prefetch({
            throttle: Number.MAX_SAFE_INTEGER
        }),
        sitemap({
            customPages: [
                "https://cotl-api.vercel.app/",
                "https://cotl-api.vercel.app/introduction",
                "https://cotl-api.vercel.app/getting-started",
                "https://cotl-api.vercel.app/items",
                "https://cotl-api.vercel.app/follower-commands",
                "https://cotl-api.vercel.app/objectives",
                "https://cotl-api.vercel.app/save-data"
            ]
        }),
        partytown({}),
        robotsTxt()
    ],
    vite: {
        build: {
            sourcemap: true
        }
    },
    markdown: {
        extendDefaultPlugins: true,
        shikiConfig: {
            theme: "one-dark-pro"
        },
        remarkPlugins: [
            [
                remarkGithub,
                {
                    repository: "https://github.com/xhayper/COTL_API.git"
                }
            ]
        ]
    },
    site: "https://cotl-api.vercel.app"
});
