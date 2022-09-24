import { defineConfig } from "astro/config";
import sitemap from "@astrojs/sitemap";
import preact from "@astrojs/preact";
import react from "@astrojs/react";

export default defineConfig({
    integrations: [preact(), react(), sitemap()],
    site: "https://xhayper.github.io",
    base: "/COTL_API"
});
