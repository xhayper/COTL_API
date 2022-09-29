import vercel from "@astrojs/vercel/serverless";
import { defineConfig } from "astro/config";
import partytown from "@astrojs/partytown";
import robotsTxt from "astro-robots-txt";
import remarkGithub from "remark-github";
import sitemap from "@astrojs/sitemap";
import preact from "@astrojs/preact";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
    output: "server",
    adapter: vercel(),
    integrations: [
        preact({ compat: true }),
        sitemap({
            customPages: [
                "https://cotl-api.vercel.app/introduction",
                "https://cotl-api.vercel.app/getting-started",
                "https://cotl-api.vercel.app/items",
                "https://cotl-api.vercel.app/follower-commands",
                "https://cotl-api.vercel.app/objectives",
                "https://cotl-api.vercel.app/save-data"
            ]
        }),
        partytown({}),
        image({
            serviceEntryPoint: "@astrojs/image/sharp"
        }),
        robotsTxt()
    ],
    vite: {
        build: {
            sourcemap: true
        }
    },
    markdown: {
        extendDefaultPlugins: true,
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
