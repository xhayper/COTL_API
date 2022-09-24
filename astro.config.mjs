import { defineConfig } from "astro/config";
import partytown from "@astrojs/partytown";
import sitemap from "@astrojs/sitemap";
import preact from "@astrojs/preact";
import react from "@astrojs/react";
import image from "@astrojs/image";

// https://astro.build/config
export default defineConfig({
  integrations: [preact(), react(), sitemap(), partytown(), image()],
  vite: {
    build: {
      sourcemap: true
    }
  },
  site: "https://xhayper.github.io",
  base: "/COTL_API"
});