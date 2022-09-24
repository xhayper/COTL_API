import { defineConfig } from "astro/config";
import partytown from "@astrojs/partytown";
import robotsTxt from 'astro-robots-txt';
import remarkGithub from "remark-github";
import sitemap from "@astrojs/sitemap";
import react from "@astrojs/react";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
    integrations: [react(), sitemap(), partytown({}), image(), robotsTxt()],
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
    site: "https://xhayper.github.io",
    base: "/COTL_API",
    trailingSlash: "ignore"
});
