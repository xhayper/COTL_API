import catppuccinMocha from "./themes/catppuccin/mocha.json";
import vercel from "@astrojs/vercel/serverless";
import { defineConfig } from "astro/config";
import robotsTxt from "astro-robots-txt";
import remarkGithub from "remark-github";
import sitemap from "@astrojs/sitemap";
import AstroPWA from "@vite-pwa/astro";
import preact from "@astrojs/preact";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
    site: "https://cotl-api.vercel.app",
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
        AstroPWA({
            manifest: {
                name: "COTL API",
                description: "Documentation for Cult of the Lamb API",
                icons: [
                    {
                        src: "/icon.svg",
                        sizes: "any"
                    }
                ],
                start_url: "/introduction",
                orientation: "landscape",
                theme_color: "#1F2937",
                background_color: "#FF1438",
                categories: ["documentation", "code", "api", "library"]
            }
        })
    ],
    vite: {
        build: {
            sourcemap: true
        }
    },
    markdown: {
        extendDefaultPlugins: true,
        shikiConfig: {
            theme: catppuccinMocha as any
        },
        remarkPlugins: [
            [
                remarkGithub,
                {
                    repository: "https://github.com/xhayper/COTL_API.git"
                }
            ]
        ]
    }
});
