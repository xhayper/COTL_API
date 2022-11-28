import catppuccinMocha from "./themes/catppuccin/mocha.json";
import vercel from "@astrojs/vercel/static";
import { defineConfig } from "astro/config";
import robotsTxt from "astro-robots-txt";
import remarkGithub from "remark-github";
import sitemap from "@astrojs/sitemap";
import compress from "astro-compress";
import preact from "@astrojs/preact";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
    site: "https://cotl-api.vercel.app",
    output: "static",
    adapter: vercel(),
    integrations: [
        preact({
            compat: true
        }),
        image({
            serviceEntryPoint: "@astrojs/image/sharp"
        }),
        sitemap({
            i18n: {
                defaultLocale: "en",
                locales: {
                    en: "en-US"
                }
            },
            customPages: [
                "https://cotl-api.vercel.app/",
                "https://cotl-api.vercel.app/follower-commands",
                "https://cotl-api.vercel.app/getting-started",
                "https://cotl-api.vercel.app/introduction",
                "https://cotl-api.vercel.app/items",
                "https://cotl-api.vercel.app/localization",
                "https://cotl-api.vercel.app/missions",
                "https://cotl-api.vercel.app/objectives",
                "https://cotl-api.vercel.app/rituals",
                "https://cotl-api.vercel.app/save-data",
                "https://cotl-api.vercel.app/skins",
                "https://cotl-api.vercel.app/sound",
                "https://cotl-api.vercel.app/structures",
                "https://cotl-api.vercel.app/tarot-cards",
                "https://cotl-api.vercel.app/tasks",
                "https://cotl-api.vercel.app/ui"
            ]
        }),
        robotsTxt({
            policy: [
                {
                    userAgent: "*",
                    allow: "/"
                }
            ]
        }),
        compress()
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
                    repository: "xhayper/COTL_API"
                }
            ]
        ]
    }
});
