import vercel from "@astrojs/vercel/serverless";
import minifyHtml from "astro-html-minifier";
import { defineConfig } from "astro/config";
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
        preact({
            compat: true
        }),
        image({
            serviceEntryPoint: "@astrojs/image/sharp"
        }),
        sitemap({
            customPages: [
                "https://cotl-api.vercel.app/",
                "https://cotl-api.vercel.app/follower-commands",
                "https://cotl-api.vercel.app/getting-started",
                "https://cotl-api.vercel.app/introduction",
                "https://cotl-api.vercel.app/items",
                "https://cotl-api.vercel.app/localization",
                "https://cotl-api.vercel.app/mission",
                "https://cotl-api.vercel.app/objectives",
                "https://cotl-api.vercel.app/ritual",
                "https://cotl-api.vercel.app/save-data",
                "https://cotl-api.vercel.app/skin",
                "https://cotl-api.vercel.app/sound",
                "https://cotl-api.vercel.app/structures",
                "https://cotl-api.vercel.app/tarot-card",
                "https://cotl-api.vercel.app/task",
                "https://cotl-api.vercel.app/ui"
            ]
        }),
        robotsTxt(),
        minifyHtml()
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
